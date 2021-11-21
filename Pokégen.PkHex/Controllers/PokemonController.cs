using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Extensions;
using Pokégen.PkHex.Models;
using Pokégen.PkHex.Services;

namespace Pokégen.PkHex.Controllers
{
	[Route("/")]
	[ApiController]
	public class PokemonController : ControllerBase
	{
		private AutoLegalityModService AutoLegalityModService { get; }
		
		private DownloaderService DownloaderService { get; }

		public PokemonController(AutoLegalityModService autoLegalityModService, DownloaderService downloaderService)
		{
			AutoLegalityModService = autoLegalityModService;
			DownloaderService = downloaderService;
		}
		
		/// <summary>
		/// Returns Pokemon Data from a Showdown set.
		/// </summary>
		/// <param name="body">Request body containing Showdown set</param>
		/// <returns>Pokemon Data as File</returns>
		/// <response code="200">Returns the Pokemon Data of Showdown set</response>
		/// <response code="400">The Pokemon set is invalid or illegal</response>     
		[HttpGet, HttpPost]
		[Route("/showdown")]
		[Produces(MediaTypeNames.Application.Octet)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetPokemonFromShowdown([FromBody, Required] PokemonShowdownRequest body)
		{
			var pkm = await GetPokemonFromShowdown(body.ShowdownSet);

			return await ReturnPokemonFile(pkm);
		}
		
		/// <summary>
		/// Checks if a Showdown set is legal
		/// </summary>
		/// <param name="body">Request body containing Showdown set</param>
		/// <returns>Nothing</returns>
		/// <response code="204">Pokemon is legal</response>
		/// <response code="400">Pokemon is illegal</response>  
		[HttpGet, HttpPost]
		[Route("/showdown/legality")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CheckShowdownLegality([FromBody, Required] PokemonShowdownRequest body)
		{
			var pkm = await GetPokemonFromShowdown(body.ShowdownSet);

			if (pkm.IsLegal())
				return NoContent();
			
			return BadRequest();
		}

		/// <summary>
		/// Returns Pokemon Data from a file by url.
		/// </summary>
		/// <param name="url">The url of the file</param>
		/// <param name="size">Optional, the size of the file if already known ahead of making a request</param>
		/// <returns>Pokemon Data of file</returns>
		/// <response code="204">Returns the Pokemon Data of the file</response>
		/// <response code="400">Pokemon is illegal or invalid</response>  
		[HttpGet]
		[Route("/url")]
		[Produces(MediaTypeNames.Application.Octet)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetPokemonFromUrl([FromQuery, Required] string url, [FromQuery] long? size)
		{
			var pkm = await DownloaderService.DownloadPkmAsync(new Uri(url), size);

			return await ReturnPokemonFile(pkm);
		}
		
		/// <summary>
		/// Checks if a Pokemon file (from an url) is legal
		/// </summary>
		/// <param name="url">The url of the file</param>
		/// <param name="size">Optional, the size of the file if already known ahead of making a request</param>
		/// <returns>Nothing</returns>
		/// <response code="204">Pokemon is legal</response>
		/// <response code="400">Pokemon is illegal</response>  
		[HttpGet]
		[Route("/url/legality")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CheckUrlLegality([FromQuery, Required] string url, [FromQuery] long? size)
		{
			var pkm = await DownloaderService.DownloadPkmAsync(new Uri(url), size);

			if (pkm.IsLegal())
				return NoContent();
			
			return BadRequest();
		}

		/// <summary>
		/// Returns Pokemon Data from an uploaded file.
		/// </summary>
		/// <param name="file">Uploaded Pokemon file</param>
		/// <returns>Pokemon Data of file</returns>
		[HttpGet, HttpPost]
		[Route("/file")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[RequestFormLimits(MultipartBodyLengthLimit = 344)]
		public async Task<IActionResult> GetPokemonFromFile([FromForm, Required] IFormFile file)
		{
			var pkm = await GetPokemonFromFormFileAsync(file);
			
			return await ReturnPokemonFile(pkm);
		}
		
		/// <summary>
		/// Checks if a Pokemon file is legal
		/// </summary>
		/// <param name="file">Uploaded Pokemon file</param>
		/// <returns>Nothing</returns>
		/// <response code="204">Pokemon is legal</response>
		/// <response code="400">Pokemon is illegal</response>  
		[HttpGet, HttpPost]
		[Route("/file/legality")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[RequestFormLimits(MultipartBodyLengthLimit = 344)]
		public async Task<IActionResult> CheckFileLegality([FromForm, Required] IFormFile file)
		{
			var pkm = await GetPokemonFromFormFileAsync(file);

			if (pkm.IsLegal())
				return NoContent();
			
			return BadRequest();
		}

		private static async Task<PKM> GetPokemonFromFormFileAsync(IFormFile file)
		{
			await using var stream = new MemoryStream();
			await file.CopyToAsync(stream);

			var pokemon = PKMConverter.GetPKMfromBytes(stream.GetBuffer(), file.FileName.Contains("pk6") ? 6 : 7);

			if (pokemon == null) throw new PokemonParseException("Couldn't parse provided file to any possible pokemon save file format.");

			return pokemon;
		}

		private Task<PKM> GetPokemonFromShowdown(string showdownSet)
		{
			var set = new ShowdownSet(showdownSet);
			var template = set.GetTemplate();

			var invalidLines = set.InvalidLines;

			if (invalidLines.Count != 0)
				return Task.FromException<PKM>(new ShowdownException($"Unable to parse Showdown Set:\n{string.Join("\n", invalidLines)}"));
			
			const int gen = 8;

			var sav = AutoLegalityModService.GetTrainerInfo(gen);

			var pkm = sav.GetLegal(template, out _);

			pkm = PKMConverter.ConvertToType(pkm, typeof(PK8), out _) ?? pkm;

			return Task.FromResult(pkm);
		}

		private Task<byte[]> CheckLegalAndGetBytes(PKM pkm) 
			=> !pkm.IsLegal() 
				? Task.FromException<byte[]>(new LegalityException("Pokemon couldn't be legalized!")) 
				: Task.FromResult(pkm.Data);
		
		private async Task<FileContentResult> ReturnPokemonFile(PKM pkm)
			=> File(await CheckLegalAndGetBytes(pkm), MediaTypeNames.Application.Octet);
	}
}

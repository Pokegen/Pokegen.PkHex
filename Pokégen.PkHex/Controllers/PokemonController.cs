using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Extensions;
using Pokégen.PkHex.Models.Requests;
using Pokégen.PkHex.Models.Responses;
using Pokégen.PkHex.Services;
using Pokégen.PkHex.Util;

namespace Pokégen.PkHex.Controllers;

/// <summary>
/// Controller which serves routes for pokemon related data
/// </summary>
[Route("pokemon/{game}")]
[ApiController]
public class PokemonController : ControllerBase
{
	private PokemonService PokemonService { get; }
		
	private DownloaderService DownloaderService { get; }

	private bool IsEncryptionWanted 
		=> (Request.Headers["X-Pokemon-Encrypted"].FirstOrDefault() ?? "").ToLower() == "true";

	/// <summary>
	/// Creates a new instance of PokemonController
	/// </summary>
	/// <param name="pokemonService">The pokemon service to use</param>
	/// <param name="downloaderService">The downloader service to use</param>
	public PokemonController(PokemonService pokemonService, DownloaderService downloaderService)
	{
		PokemonService = pokemonService;
		DownloaderService = downloaderService;
	}

	/// <summary>
	/// Returns Pokemon Data from a Showdown set.
	/// </summary>
	/// <param name="body">Request body containing Showdown set</param>
	/// <param name="game">Wanted game for the returned file format</param>
	/// <returns>Pokemon Data as File</returns>
	/// <response code="200">Returns the Pokemon Data of Showdown set</response>
	/// <response code="400">The Pokemon set is invalid or illegal</response>     
	[HttpPost]
	[Route("showdown")]
	[Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetPokemonFromShowdown([FromRoute] string game, [FromBody, Required] PokemonShowdownRequest body)
	{
		var pkm = await PokemonService.GetPokemonFromShowdown(body.ShowdownSet, SupportGameUtil.GetFromString(game));

		return await ReturnPokemonFile(pkm, IsEncryptionWanted);
	}

	/// <summary>
	/// Checks if a Showdown set is legal
	/// </summary>
	/// <param name="body">Request body containing Showdown set</param>
	/// <param name="game">Wanted game for the returned file format</param>
	/// <returns>Nothing</returns>
	/// <response code="204">Pokemon is legal</response>
	/// <response code="400">Pokemon is illegal</response>  
	[HttpPost]
	[Route("showdown/legality")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CheckShowdownLegality([FromRoute] string game, [FromBody, Required] PokemonShowdownRequest body)
	{
		var pkm = await PokemonService.GetPokemonFromShowdown(body.ShowdownSet, SupportGameUtil.GetFromString(game));

		if (pkm.IsLegal())
			return NoContent();
			
		throw new LegalityException("Pokemon couldn't be legalized!");
	}

	/// <summary>
	/// Creates a Summary of a Showdown Set
	/// </summary>
	/// <param name="body">Request body containing Showdown set</param>
	/// <param name="game">Wanted game for the Showdown Set -> PKM conversion</param>
	/// <returns>Nothing</returns>
	/// <response code="200">Summary of Pokemon</response>
	/// <response code="400">Showdown Set is invalid</response> 
	/// <returns>Summary of Pokemon</returns>
	[HttpPost]
	[Route("showdown/summary")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<PokemonSummary> GetSummaryFromShowdown([FromRoute] string game,
		[FromBody, Required] PokemonShowdownRequest body)
	{
		var pkm = await PokemonService.GetPokemonFromShowdown(body.ShowdownSet, SupportGameUtil.GetFromString(game));

		return pkm.ToSummary();
	}

	/// <summary>
	/// Returns Pokemon Data from a file by url.
	/// </summary>
	/// <param name="game">Wanted game for the returned file format</param>
	/// <param name="url">The url of the file</param>
	/// <param name="size">Optional, the size of the file if already known ahead of making a request</param>
	/// <returns>Pokemon Data of file</returns>
	/// <response code="200">Returns the Pokemon Data of the file</response>
	/// <response code="400">Pokemon is illegal or invalid</response>  
	[HttpGet("url")]
	[Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetPokemonFromUrl([FromRoute] string game, [FromQuery, Required] string url, [FromQuery] long? size)
	{
		var pkm = await DownloaderService.DownloadPkmAsync(new Uri(url), size, SupportGameUtil.GetFromString(game));

		return await ReturnPokemonFile(pkm, IsEncryptionWanted);
	}

	/// <summary>
	/// Checks if a Pokemon file (from an url) is legal
	/// </summary>
	/// <param name="game">Wanted game for the returned file format</param>
	/// <param name="url">The url of the file</param>
	/// <param name="size">Optional, the size of the file if already known ahead of making a request</param>
	/// <returns>Nothing</returns>
	/// <response code="204">Pokemon is legal</response>
	/// <response code="400">Pokemon is illegal</response>  
	[HttpGet("url/legality")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CheckUrlLegality([FromRoute] string game, [FromQuery, Required] string url, [FromQuery] long? size)
	{
		var pkm = await DownloaderService.DownloadPkmAsync(new Uri(url), size, SupportGameUtil.GetFromString(game));

		if (pkm.IsLegal())
			return NoContent();
			
		throw new LegalityException("Pokemon couldn't be legalized!");
	}
	
	/// <summary>
	/// Returns Pokemon Summary from a file by url.
	/// </summary>
	/// <param name="game">Wanted game for the Summary</param>
	/// <param name="url">The url of the file</param>
	/// <param name="size">Optional, the size of the file if already known ahead of making a request</param>
	/// <returns>Pokemon Data of file</returns>
	/// <response code="200">Returns the Pokemon Summary of the file</response>
	/// <response code="400">Pokemon file is invalid</response>  
	[HttpGet("url/summary")]
	[Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<PokemonSummary> GetPokemonSummaryFromUrl([FromRoute] string game, [FromQuery, Required] string url, [FromQuery] long? size)
	{
		var pkm = await DownloaderService.DownloadPkmAsync(new Uri(url), size, SupportGameUtil.GetFromString(game));

		return pkm.ToSummary();
	}
	

	/// <summary>
	/// Returns Pokemon Data from an uploaded file.
	/// </summary>
	/// <param name="game">Wanted game for the returned file format</param>
	/// <param name="file">Uploaded Pokemon file</param>
	/// <returns>Pokemon Data of file</returns>
	[HttpPost]
	[Route("file")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[RequestFormLimits(MultipartBodyLengthLimit = 376)]
	public async Task<IActionResult> GetPokemonFromFile([FromRoute] string game, [Required] IFormFile file)
	{
		var pkm = await PokemonService.GetPokemonFromFormFileAsync(file, SupportGameUtil.GetFromString(game));
			
		return await ReturnPokemonFile(pkm, IsEncryptionWanted);
	}

	/// <summary>
	/// Checks if a Pokemon file is legal
	/// </summary>
	/// <param name="game">Wanted game for the returned file format</param>
	/// <param name="file">Uploaded Pokemon file</param>
	/// <returns>Nothing</returns>
	/// <response code="204">Pokemon is legal</response>
	/// <response code="400">Pokemon is illegal</response>  
	[HttpPost]
	[Route("file/legality")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[RequestFormLimits(MultipartBodyLengthLimit = 376)]
	public async Task<IActionResult> CheckFileLegality([FromRoute] string game, [Required] IFormFile file)
	{
		var pkm = await PokemonService.GetPokemonFromFormFileAsync(file, SupportGameUtil.GetFromString(game));

		if (pkm.IsLegal())
			return NoContent();
			
		throw new LegalityException("Pokemon couldn't be legalized!");
	}
	
	/// <summary>
	/// Returns Pokemon Summary from an uploaded file.
	/// </summary>
	/// <param name="game">Wanted game for the Summary</param>
	/// <param name="file">Uploaded Pokemon file</param>
	/// <returns>Pokemon Summary of file</returns>
	[HttpPost]
	[Route("file/summary")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[RequestFormLimits(MultipartBodyLengthLimit = 376)]
	public async Task<PokemonSummary> GetPokemonSummaryFromFile([FromRoute] string game, [Required] IFormFile file)
	{
		var pkm = await PokemonService.GetPokemonFromFormFileAsync(file, SupportGameUtil.GetFromString(game));

		return pkm.ToSummary();
	}
	
	private async Task<IActionResult> ReturnPokemonFile(PKM pkm, bool encrypted = false)
	{
		Response.Headers.Add("X-Pokemon-Species", ((Species) pkm.Species).ToString());
		Response.Headers.Add("X-Pokemon-Language", ((LanguageID) pkm.Language).ToString());
		if (pkm is ISanityChecksum sanityChecksum) Response.Headers.Add("X-Pokemon-Checksum", sanityChecksum.Checksum.ToString());

		pkm.ResetPartyStats();

		return File(await PokemonService.CheckLegalAndGetBytes(pkm, encrypted), MediaTypeNames.Application.Octet);
	}
}

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Extensions;

namespace Pokégen.PkHex.Services;

public class PokemonService
{
	private AutoLegalityModService AutoLegalityModService { get; }

	public PokemonService(AutoLegalityModService autoLegalityModService) 
		=> AutoLegalityModService = autoLegalityModService;

	public async Task<PKM> GetPokemonFromFormFileAsync(IFormFile file)
	{
		await using var stream = new MemoryStream();
		await file.CopyToAsync(stream);

		var pokemon = PKMConverter.GetPKMfromBytes(stream.GetBuffer(), file.FileName.Contains("pk6") ? 6 : 7);

		if (pokemon == null) throw new PokemonParseException("Couldn't parse provided file to any possible pokemon save file format.");

		return pokemon;
	}

	public Task<PKM> GetPokemonFromShowdown(string showdownSet)
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

	public Task<byte[]> CheckLegalAndGetBytes(PKM pkm) 
		=> !pkm.IsLegal() 
			? Task.FromException<byte[]>(new LegalityException("Pokemon couldn't be legalized!")) 
			: Task.FromResult(pkm.Data);
}

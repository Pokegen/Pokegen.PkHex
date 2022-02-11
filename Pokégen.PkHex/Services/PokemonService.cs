using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Extensions;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Services;

public class PokemonService
{
	private AutoLegalityModService AutoLegalityModService { get; }

	public PokemonService(AutoLegalityModService autoLegalityModService) 
		=> AutoLegalityModService = autoLegalityModService;

	public async Task<PKM> GetPokemonFromFormFileAsync(IFormFile file, SupportedGame game)
	{
		await using var stream = new MemoryStream();
		await file.CopyToAsync(stream);

		var pokemon = PKMConverter.GetPKMfromBytes(stream.GetBuffer(), file.FileName.Contains("pk6") ? 6 : 7);

		if (pokemon == null) throw new PokemonParseException("Couldn't parse provided file to any possible pokemon save file format.");

		var correctGame = game switch {
			SupportedGame.SWSH => pokemon is PK8,
			SupportedGame.BDSP => pokemon is PB8,
			SupportedGame.PLA => pokemon is PA8,
			_ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
		};

		if (!correctGame) throw new LegalityException("Invalid Game Version");

		return pokemon;
	}

	public Task<PKM> GetPokemonFromShowdown(string showdownSet, SupportedGame game)
	{
		var set = new ShowdownSet(showdownSet);
		var template = set.GetTemplate();

		var invalidLines = set.InvalidLines;

		if (invalidLines.Count != 0)
			return Task.FromException<PKM>(new ShowdownException($"Unable to parse Showdown Set:\n{string.Join("\n", invalidLines)}"));

		var sav = game switch
		{
			SupportedGame.SWSH => AutoLegalityModService.GetTrainerInfo<PK8>(),
			SupportedGame.BDSP => AutoLegalityModService.GetTrainerInfo<PB8>(),
			SupportedGame.PLA => AutoLegalityModService.GetTrainerInfo<PA8>(),
			_ => throw new ArgumentOutOfRangeException(nameof(game))
		};

		var pkm = sav.GetLegal(template, out _);

		pkm = game switch
		{
			SupportedGame.SWSH => PKMConverter.ConvertToType(pkm, typeof(PK8), out _) ?? pkm,
			SupportedGame.BDSP => PKMConverter.ConvertToType(pkm, typeof(PB8), out _) ?? pkm,
			SupportedGame.PLA => PKMConverter.ConvertToType(pkm, typeof(PA8), out _) ?? pkm,
			_ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
		};

		return Task.FromResult(pkm);
	}

	public Task<byte[]> CheckLegalAndGetBytes(PKM pkm, bool encrypted)
		=> !pkm.IsLegal() 
			? Task.FromException<byte[]>(new LegalityException("Pokemon couldn't be legalized!")) 
			: Task.FromResult(encrypted ? pkm.EncryptedPartyData : pkm.DecryptedPartyData);
}

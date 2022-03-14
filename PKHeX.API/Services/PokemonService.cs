using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PKHeX.Core;
using PKHeX.Core.AutoMod;
using PKHeX.API.Exceptions;
using PKHeX.API.Models;
using static PKHeX.API.Util.Util;

namespace PKHeX.API.Services;

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

		if (pokemon == null) throw new BadRequestException("Couldn't parse provided file to any possible pokemon save file format.");

		var correctGame = game switch {
			SupportedGame.LGPE => pokemon is PB7,
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
		var template = new RegenTemplate(set);
		var ot = GetEnvOrThrow("PKHEX_DEFAULT_OT");

		var sav = game switch
		{
			SupportedGame.LGPE => SaveUtil.GetBlankSAV(GameVersion.GE, ot),
			SupportedGame.SWSH => SaveUtil.GetBlankSAV(GameVersion.SWSH, ot),
			SupportedGame.BDSP => SaveUtil.GetBlankSAV(GameVersion.BD, ot),
			SupportedGame.PLA => SaveUtil.GetBlankSAV(GameVersion.PLA, ot),
			_ => throw new ArgumentOutOfRangeException(nameof(game))
		};

		var pkm = sav.GetLegal(template, out _);

		pkm = game switch
		{
			SupportedGame.LGPE => PKMConverter.ConvertToType(pkm, typeof(PB7), out _) ?? pkm,
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
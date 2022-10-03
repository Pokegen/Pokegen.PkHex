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

		var entityContext = EntityFileExtension.GetContextFromExtension(file.FileName);
		var pokemon = EntityFormat.GetFromBytes(stream.GetBuffer(), entityContext);

		if (pokemon == null) throw new PokemonParseException("Couldn't parse provided file to any possible pokemon save file format.");

		var correctGame = game switch {
			SupportedGame.RBY => pokemon is PK1,
			SupportedGame.GSC => pokemon is PK2,
			SupportedGame.RS or SupportedGame.E => pokemon is PK3,
			SupportedGame.DP or SupportedGame.PT or SupportedGame.HGSS => pokemon is PK4,
			SupportedGame.BW or SupportedGame.BW2 => pokemon is PK5,
			SupportedGame.XY or SupportedGame.ORAS => pokemon is PK6,
			SupportedGame.USUM or SupportedGame.SM => pokemon is PK7,
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
		var template = set.GetTemplate();

		var invalidLines = set.InvalidLines;

		if (invalidLines.Count != 0)
			return Task.FromException<PKM>(new ShowdownException($"Unable to parse Showdown Set:\n{string.Join("\n", invalidLines)}"));

		var sav = game switch
		{
			SupportedGame.RBY => AutoLegalityModService.GetTrainerInfo<PK1>(),
			SupportedGame.GSC => AutoLegalityModService.GetTrainerInfo<PK2>(),
			SupportedGame.RS or SupportedGame.E => AutoLegalityModService.GetTrainerInfo<PK3>(),
			SupportedGame.DP or SupportedGame.PT or SupportedGame.HGSS => AutoLegalityModService.GetTrainerInfo<PK4>(),
			SupportedGame.BW or SupportedGame.BW2 => AutoLegalityModService.GetTrainerInfo<PK5>(),
			SupportedGame.XY or SupportedGame.ORAS => AutoLegalityModService.GetTrainerInfo<PK6>(),
			SupportedGame.SM or SupportedGame.USUM => AutoLegalityModService.GetTrainerInfo<PK7>(),
			SupportedGame.LGPE => AutoLegalityModService.GetTrainerInfo<PB7>(),
			SupportedGame.SWSH => AutoLegalityModService.GetTrainerInfo<PK8>(),
			SupportedGame.BDSP => AutoLegalityModService.GetTrainerInfo<PB8>(),
			SupportedGame.PLA => AutoLegalityModService.GetTrainerInfo<PA8>(),
			_ => throw new ArgumentOutOfRangeException(nameof(game))
		};

		var pkm = sav.GetLegal(template, out _);

		pkm = ConvertToWantedType(pkm, game);

		return Task.FromResult(pkm);
	}

	public Task<byte[]> CheckLegalAndGetBytes(PKM pkm, bool encrypted)
		=> !pkm.IsLegal() 
			? Task.FromException<byte[]>(new LegalityException("Pokemon couldn't be legalized!")) 
			: Task.FromResult(encrypted ? pkm.EncryptedPartyData : pkm.DecryptedPartyData);

	public PKM ConvertToWantedType(PKM pkm, SupportedGame game) 
		=> game switch
		{
			SupportedGame.RBY => EntityConverter.ConvertToType(pkm, typeof(PK1), out _) ?? pkm,
			SupportedGame.GSC => EntityConverter.ConvertToType(pkm, typeof(PK2), out _) ?? pkm,
			SupportedGame.RS or SupportedGame.E => EntityConverter.ConvertToType(pkm, typeof(PK3), out _) ?? pkm,
			SupportedGame.DP or SupportedGame.PT or SupportedGame.HGSS => EntityConverter.ConvertToType(pkm, typeof(PK4), out _) ?? pkm,
			SupportedGame.BW or SupportedGame.BW2 => EntityConverter.ConvertToType(pkm, typeof(PK5), out _) ?? pkm,
			SupportedGame.XY or SupportedGame.ORAS => EntityConverter.ConvertToType(pkm, typeof(PK6), out _) ?? pkm,
			SupportedGame.SM or SupportedGame.USUM => EntityConverter.ConvertToType(pkm, typeof(PK7), out _) ?? pkm,
			SupportedGame.LGPE => EntityConverter.ConvertToType(pkm, typeof(PB7), out _) ?? pkm,
			SupportedGame.SWSH => EntityConverter.ConvertToType(pkm, typeof(PK8), out _) ?? pkm,
			SupportedGame.BDSP => EntityConverter.ConvertToType(pkm, typeof(PB8), out _) ?? pkm,
			SupportedGame.PLA => EntityConverter.ConvertToType(pkm, typeof(PA8), out _) ?? pkm,
			_ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
		};
}

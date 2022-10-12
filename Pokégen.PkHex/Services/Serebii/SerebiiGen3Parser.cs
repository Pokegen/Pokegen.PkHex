using System;
using System.Collections.Generic;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using PKHeX.Core;
using Pokégen.PkHex.Models;
using Pokégen.PkHex.Util;

namespace Pokégen.PkHex.Services.Serebii;

public class SerebiiGen3Parser : ISerebiiGenerationParser
{
	public int Generation => 3;
	
	private IBrowsingContext BrowsingContext { get; }

	private IHtmlParser HtmlParser { get; }


	public SerebiiGen3Parser(IBrowsingContext browsingContext)
	{
		BrowsingContext = browsingContext;
		HtmlParser = BrowsingContext.GetService<IHtmlParser>()!;
	}
	
	public IEnumerable<BasicPokemonGameInfo> ParseHtmlToPokemons(string html)
	{
		var document = HtmlParser.ParseDocument(html);

		var name = document.QuerySelector(
			"#content > main > div > table:nth-child(5) > tbody > tr:nth-child(2) > td:nth-child(4)")?.Text()!;
		
		var splashArt = document.QuerySelector("#rbar > table.art > tbody > tr > td > img")?.GetAttribute("src")!;

		var species = Enum.Parse<Species>(SerebiiUtil.NormalizePokemonNames(name), true);
		
		return new[]
		{
			new BasicPokemonGameInfo(SupportedGame.RS, new BasicPokemonInfo
			{
				Species = species
			}),
			new BasicPokemonGameInfo(SupportedGame.E, new BasicPokemonInfo
			{
				Species = species
			}),
			new BasicPokemonGameInfo(SupportedGame.FRLG, new BasicPokemonInfo
			{
				Species = species
			})
		};
	}
}

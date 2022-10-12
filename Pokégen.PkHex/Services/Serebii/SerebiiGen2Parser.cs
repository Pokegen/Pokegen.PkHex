using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using PKHeX.Core;
using Pokégen.PkHex.Models;
using Pokégen.PkHex.Util;

namespace Pokégen.PkHex.Services.Serebii;

public class SerebiiGen2Parser : ISerebiiGenerationParser
{
	public int Generation => 2;
	
	private IBrowsingContext BrowsingContext { get; }

	private IHtmlParser HtmlParser { get; }


	public SerebiiGen2Parser(IBrowsingContext browsingContext)
	{
		BrowsingContext = browsingContext;
		HtmlParser = BrowsingContext.GetService<IHtmlParser>()!;
	}

	public IEnumerable<BasicPokemonGameInfo> ParseHtmlToPokemons(string html)
	{
		var document = HtmlParser.ParseDocument(html);

		var name = SerebiiUtil.NormalizePokemonNames(document.QuerySelector(
			"#content > main > div > div > table:nth-child(5) > tbody > tr:nth-child(2) > td:nth-child(1)")?.Text()!);

		var splashArt = document.QuerySelector("#rbar > table.art > tbody > tr > td > img")?.GetAttribute("src")!;
		var imageGold =
			document.QuerySelector(
				"#content > main > div > div > table:nth-child(4) > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(1) > td:nth-child(1) > img")?.GetAttribute("src")!;
		var imageGoldShiny =
			document.QuerySelector(
				"#content > main > div > div > table:nth-child(4) > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(2) > td:nth-child(1) > img")?.GetAttribute("src")!;
		var imageSilver = document.QuerySelector("#content > main > div > div > table:nth-child(4) > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(1) > td:nth-child(2) > img")?.GetAttribute("src")!;
		var imageSilverShiny =
			document.QuerySelector(
				"#content > main > div > div > table:nth-child(4) > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(2) > td:nth-child(2) > img")?.GetAttribute("src")!;
		var imageCrystal = document.QuerySelector("#content > main > div > div > table:nth-child(4) > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(1) > td:nth-child(3) > img")?.GetAttribute("src")!;
		var imageCrystalShiny =
			document.QuerySelector(
				"#content > main > div > div > table:nth-child(4) > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(2) > td:nth-child(3) > img")?.GetAttribute("src")!;
		
		var moves = document.QuerySelectorAll(".fooinfo > a")
			.Select(element => Enum.Parse<Move>(SerebiiUtil.NormalizeAttackNames(element.Text()), true))
			.ToList();

		return new []
		{
			new BasicPokemonGameInfo(SupportedGame.GSC, new BasicPokemonInfo
			{
				Species = Enum.Parse<Species>(name, true),
				Images = new[] { imageGold, imageSilver, imageCrystal, splashArt }.Select(url =>
					"https://www.serebii.net" + url),
				ShinyImages =
					new[] { imageGoldShiny, imageSilverShiny, imageCrystalShiny }.Select(url =>
						"https://www.serebii.net" + url),
				Moves = moves
			})
		};
	}
}

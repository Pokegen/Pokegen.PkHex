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

public class SerebiiGen1Parser : ISerebiiGenerationParser
{
	public int Generation => 1;
	
	private IBrowsingContext BrowsingContext { get; }

	private IHtmlParser HtmlParser { get; }


	public SerebiiGen1Parser(IBrowsingContext browsingContext)
	{
		BrowsingContext = browsingContext;
		HtmlParser = BrowsingContext.GetService<IHtmlParser>()!;
	}

	public IEnumerable<BasicPokemonGameInfo> ParseHtmlToPokemons(string html)
	{
		var document = HtmlParser.ParseDocument(html);

		var name = SerebiiUtil.NormalizePokemonNames(document.QuerySelector(
				"#content > main > div > div > table:nth-child(5) > tbody > tr:nth-child(2) > td:nth-child(1)")?
			.Text()!);
		
		// Images
		var imageGreen = document.QuerySelector("#sprite-g")?.GetAttribute("src");
		var imageRedBlue = document.QuerySelector("#sprite-rb")?.GetAttribute("src");
		var imageYellow = document.QuerySelector("#sprite-y")?.GetAttribute("src");
		var splashArt = document.QuerySelector("#rbar > table.art > tbody > tr > td > img")?.GetAttribute("src");

		var moves = document.QuerySelectorAll(".fooinfo > a")
			.Select(element => Enum.Parse<Move>(SerebiiUtil.NormalizeAttackNames(element.Text()), true))
			.ToList();

		return new[]
		{
			new BasicPokemonGameInfo(SupportedGame.RBY, new BasicPokemonInfo
			{
				Species = Enum.Parse<Species>(name, true),
				Images = new[] { imageGreen, imageRedBlue, imageYellow, splashArt }.Select(url =>
					"https://www.serebii.net" + url),
				Moves = moves
			})
		};
	}
}

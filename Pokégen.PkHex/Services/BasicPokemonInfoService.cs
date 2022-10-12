using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PKHeX.Core;
using Pokégen.PkHex.Models;
using Pokégen.PkHex.Services.Serebii;
using Pokégen.PkHex.Util;

namespace Pokégen.PkHex.Services;

public class BasicPokemonInfoService : IHostedService
{
	public Dictionary<SupportedGame, IList<BasicPokemonInfo>> BasicPokemonInfos { get; } = new();

	private IEnumerable<ISerebiiGenerationParser> Parsers { get; }

	private ILogger<BasicPokemonInfoService> Logger { get; }
	
	private HttpClient HttpClient { get; }
	
	public BasicPokemonInfoService(ILogger<BasicPokemonInfoService> logger, HttpClient httpClient, IEnumerable<ISerebiiGenerationParser> parsers)
	{
		Logger = logger;
		HttpClient = httpClient;
		Parsers = parsers;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		await ScrapePokemonFromSerebii(cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
		=> Task.CompletedTask;

	private async Task ScrapePokemonFromSerebii(CancellationToken cancellationToken)
	{
		var mapping = new Dictionary<int, SupportedGame[]>
		{
			{ 1, new [] { SupportedGame.RBY } },
			{ 2, new [] { SupportedGame.GSC } },
			{ 3, new [] { SupportedGame.RS, SupportedGame.E, SupportedGame.FRLG } },
			{ 4, new [] { SupportedGame.DP, SupportedGame.PT, SupportedGame.HGSS } },
			{ 5, new [] { SupportedGame.BW, SupportedGame.BW2 } },
			{ 6, new [] { SupportedGame.XY, SupportedGame.ORAS } },
			{ 7, new [] { SupportedGame.SM, SupportedGame.USUM, SupportedGame.LGPE} },
			{ 8, new [] { SupportedGame.SWSH, SupportedGame.BDSP, SupportedGame.LA } },
			{ 9, new [] {SupportedGame.SV } }
		};
		
		for (var i = 1; i <= PKX.Generation; i++)
		{
			var parser = Parsers.FirstOrDefault(parser => parser.Generation == i);

			if (parser == null)
			{
				Logger.LogWarning("Missing Parser for Generation: {Generation}", i);
				continue;
			}

			foreach (var game in mapping[i]) 
				BasicPokemonInfos.Add(game, new List<BasicPokemonInfo>());

			var i1 = i;
			var tasks = Enum.GetValues<Species>()
				.Where(species => species != Species.None && i switch
				{
					1 => PersonalTable.RB.IsSpeciesInGame((ushort)species),
					2 => PersonalTable.C.IsSpeciesInGame((ushort)species),
					3 => PersonalTable.RS.IsSpeciesInGame((ushort)species),
					4 => PersonalTable.Pt.IsSpeciesInGame((ushort)species),
					5 => PersonalTable.B2W2.IsSpeciesInGame((ushort)species),
					6 => PersonalTable.AO.IsSpeciesInGame((ushort)species),
					7 => PersonalTable.USUM.IsSpeciesInGame((ushort)species),
					8 => PersonalTable.SWSH.IsSpeciesInGame((ushort)species),
					_ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
				})
				.Select(species => Task.Run((async () =>
				{
					var response = await HttpClient.GetAsync(SerebiiUtil.GetUrlFromGenerationAndSpecies(i1, species), cancellationToken);

					response.EnsureSuccessStatusCode();

					var html = await response.Content.ReadAsStringAsync(cancellationToken);

					var parsedPokemons = parser.ParseHtmlToPokemons(html);;

					foreach (var (game, info) in parsedPokemons) 
						BasicPokemonInfos[game].Add(info);

				}), cancellationToken))
				.ToList();

			await Task.WhenAll(tasks);

			foreach (var (supportedGame, list) in BasicPokemonInfos)
				BasicPokemonInfos[supportedGame] = list.OrderBy(p => p.Species).ToList();
		}
	}
}

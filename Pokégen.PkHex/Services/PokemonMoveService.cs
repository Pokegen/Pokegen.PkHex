using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PKHeX.Core;
using PKHeX.Core.AutoMod;
using Pokégen.PkHex.Extensions;
using Pokégen.PkHex.Models;
using Pokégen.PkHex.Util;
using static Pokégen.PkHex.Util.Util;

namespace Pokégen.PkHex.Services;

public class PokemonMoveService : IHostedService
{
	public Dictionary<SupportedGame, Dictionary<Species, IList<Move>>> PossibleMovesInSupportedGame { get; } = new();

	private ILogger<PokemonMoveService> Logger { get; }
	
	public PokemonMoveService(ILogger<PokemonMoveService> logger) 
		=> Logger = logger;

	public Task StartAsync(CancellationToken cancellationToken)
	{
		Task.Run(() => GeneratePossibleMoves(cancellationToken).RunSynchronously(), cancellationToken: cancellationToken);

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
		=> Task.CompletedTask;

	private async Task GeneratePossibleMoves(CancellationToken cancellationToken)
	{
		var factory = new TaskFactory(
			cancellationToken,
			TaskCreationOptions.None,
			TaskContinuationOptions.None,
			new ThreadPoolTaskScheduler(GetEnvAsIntOrDefault("POKEMON_MOVE_GENERATOR_MAX_THREADS", 50)));
		
		foreach (var game in Enum.GetValues<SupportedGame>())
		{
			var speciesTasks = Enum.GetValues<Species>()
				.Where(species => species != Species.None && game switch
				{
					SupportedGame.RBY => PersonalTable.RB.IsSpeciesInGame((ushort) species),
					SupportedGame.GSC => PersonalTable.GS.IsSpeciesInGame((ushort) species),
					SupportedGame.RS or SupportedGame.E => PersonalTable.RS.IsSpeciesInGame((ushort) species),
					SupportedGame.DP or SupportedGame.PT or SupportedGame.HGSS => PersonalTable.DP.IsSpeciesInGame((ushort) species),
					SupportedGame.BW or SupportedGame.BW2 => PersonalTable.BW.IsSpeciesInGame((ushort) species),
					SupportedGame.XY or SupportedGame.ORAS => PersonalTable.XY.IsSpeciesInGame((ushort) species),
					SupportedGame.USUM or SupportedGame.SM => PersonalTable.SM.IsSpeciesInGame((ushort) species),
					SupportedGame.LGPE => PersonalTable.GG.IsSpeciesInGame((ushort) species),
					SupportedGame.SWSH => PersonalTable.SWSH.IsSpeciesInGame((ushort) species),
					SupportedGame.BDSP => PersonalTable.BDSP.IsSpeciesInGame((ushort) species),
					SupportedGame.LA => PersonalTable.LA.IsSpeciesInGame((ushort) species),
					_ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
				})
				.Select(species => factory.StartNew(() =>
				{
					var legalMoves = new List<Move>();
					
					PKM pkm = game switch
					{
						SupportedGame.RBY => new PK1(),
						SupportedGame.GSC => new PK2(),
						SupportedGame.RS or SupportedGame.E => new PK3(),
						SupportedGame.DP or SupportedGame.PT or SupportedGame.HGSS => new PK4(),
						SupportedGame.BW or SupportedGame.BW2 => new PK5(),
						SupportedGame.XY or SupportedGame.ORAS => new PK6(),
						SupportedGame.USUM or SupportedGame.SM => new PK7(),
						SupportedGame.LGPE => new PB7(),
						SupportedGame.SWSH => new PK8(),
						SupportedGame.BDSP => new PB8(),
						SupportedGame.LA => new PA8(),
						_ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
					};

					pkm.Species = (ushort)species;
					pkm.CurrentLevel = 100;

					foreach (var move in Enum.GetValues<Move>())
					{
						if (cancellationToken.IsCancellationRequested) break;

						pkm.Move1 = (ushort)move;

						try
						{
							pkm = pkm.Legalize();

							if (pkm.IsLegal()) legalMoves.Add(move);
						}
						catch (Exception e)
						{
							// Ignored
						}
					}
					
					if (!cancellationToken.IsCancellationRequested)
						Logger.LogInformation("Done with {Species}", species);

					return (species, legalMoves);
				}, cancellationToken))
				.ToList();

			await Task.WhenAll(speciesTasks);

			var legalMoves = new Dictionary<Species, IList<Move>>();
			
			foreach (var speciesTask in speciesTasks)
			{
				var (species, moves) = speciesTask.Result;
				legalMoves.Add(species, moves);
			}
			
			PossibleMovesInSupportedGame.Add(game, legalMoves);
			Logger.LogInformation("Done with Game {Game}", game);
		}
	}
}

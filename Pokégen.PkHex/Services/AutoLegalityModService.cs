using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PKHeX.Core;
using PKHeX.Core.AutoMod;
using static Pokégen.PkHex.Util.Util;

namespace Pokégen.PkHex.Services;

public class AutoLegalityModService : IHostedService
{
	private bool Initialized { get; set; }

	public AutoLegalityModService()
	{
		SetupSettings();
		InitializeCoreStrings();
	}

	public Task StartAsync(CancellationToken cancellationToken) 
		=> Initialized ? Task.CompletedTask : InitializeAsync(cancellationToken);

	public Task StopAsync(CancellationToken cancellationToken) 
		=> Task.CompletedTask;

	public ITrainerInfo GetTrainerInfo<T>() where T : PKM
	{
		if (typeof(T) == typeof(PK1))
			return TrainerSettings.GetSavedTrainerData(GameVersion.R, 1);
		if (typeof(T) == typeof(PK2))
			return TrainerSettings.GetSavedTrainerData(GameVersion.C, 2);
		if (typeof(T) == typeof(PK3))
			return TrainerSettings.GetSavedTrainerData(GameVersion.S, 3);
		if (typeof(T) == typeof(PK4))
			return TrainerSettings.GetSavedTrainerData(GameVersion.Pt, 4);
		if (typeof(T) == typeof(PK5))
			return TrainerSettings.GetSavedTrainerData(GameVersion.B2W2, 5);
		if (typeof(T) == typeof(PK6))
			return TrainerSettings.GetSavedTrainerData(GameVersion.XY, 6);
		if (typeof(T) == typeof(PK7))
			return TrainerSettings.GetSavedTrainerData(GameVersion.USUM, 7);
		if (typeof(T) == typeof(PB7))
			return TrainerSettings.GetSavedTrainerData(GameVersion.GP, 7);
		if (typeof(T) == typeof(PK8))
			return TrainerSettings.GetSavedTrainerData(GameVersion.SWSH, 8);
		if (typeof(T) == typeof(PB8))
			return TrainerSettings.GetSavedTrainerData(GameVersion.BDSP, 8);
		if (typeof(T) == typeof(PA8))
			return TrainerSettings.GetSavedTrainerData(GameVersion.PLA, 8);
		
		throw new ArgumentException("Type does not have a recognized trainer fetch.", typeof(T).Name);
	}

	private async Task InitializeAsync(CancellationToken cancellationToken)
	{
		await Task.WhenAll(
			Task.Run(() => EncounterEvent.RefreshMGDB("./mgdb"), cancellationToken),
			Task.Run(InitializeTrainerDatabase, cancellationToken));
		Initialized = true;
	}

	private static void SetupSettings()
	{
		APILegality.SetAllLegalRibbons = GetEnvAsBoolOrDefault("ALM_SET_ALL_LEGAL_RIBBONS", true);
		APILegality.SetMatchingBalls = GetEnvAsBoolOrDefault("ALM_SET_MATCHING_BALLS", true);
		APILegality.ForceSpecifiedBall = GetEnvAsBoolOrDefault("ALM_FORCE_SPECIFIC_BALL", true);
		APILegality.UseXOROSHIRO = GetEnvAsBoolOrDefault("ALM_USE_XOROSHIRO", true);
		APILegality.AllowTrainerOverride = GetEnvAsBoolOrDefault("ALM_ALLOW_TRAINER_OVERRIDE", true);
		APILegality.AllowBatchCommands = GetEnvAsBoolOrDefault("ALM_ALLOW_BATCH_COMMAND", true);
		APILegality.PrioritizeGame = GetEnvAsBoolOrDefault("ALM_PRIORITIZE_GAME", true);
		APILegality.PrioritizeGameVersion= GetEnvAsEnumOrDefault<GameVersion>("ALM_PRIORITIZE_GAME_VERSION", GameVersion.Any);
		APILegality.SetBattleVersion = GetEnvAsBoolOrDefault("ALM_SET_BATTLE_VERSION", false);
		APILegality.Timeout = GetEnvAsIntOrDefault("ALM_TIMEOUT", 15);
		Legalizer.EnableEasterEggs = GetEnvAsBoolOrDefault("ALM_ENABLE_EASTER_EGGS", false);
	}

	private static void InitializeTrainerDatabase()
	{
		var ot = GetEnvOrThrow("PKHEX_DEFAULT_OT");
		var trainerId = GetEnvAsIntOrThrow("PKHEX_DEFAULT_TID");
		var secretId = GetEnvAsIntOrThrow("PKHEX_DEFAULT_SID");
		var languageName = GetEnvOrThrow("PKHEX_DEFAULT_LANGUAGE");
	        
		if (!Enum.TryParse<LanguageID>(languageName, true, out var language))
			throw new Exception($"Invalid default language {languageName}");

		for (var i = 1; i < PKX.Generation + 1; i++)
		{
			var versions = GameUtil.GetVersionsInGeneration(i, PKX.Generation);
			foreach (var v in versions)
			{
				var fallback = new SimpleTrainerInfo(v)
				{
					Language = (int)language,
					TID = trainerId,
					SID = secretId,
					OT = ot,
				};
				var exist = TrainerSettings.GetSavedTrainerData(v, i, fallback);
				if (exist is SimpleTrainerInfo)
					TrainerSettings.Register(fallback);
			}
		}

		var trainer = TrainerSettings.GetSavedTrainerData(PKX.Generation);
		RecentTrainerCache.SetRecentTrainer(trainer);
	}

	private static void InitializeCoreStrings()
	{
		var lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName[..2];
		LocalizationUtil.SetLocalization(typeof(LegalityCheckStrings), lang);
		LocalizationUtil.SetLocalization(typeof(MessageStrings), lang);
		RibbonStrings.ResetDictionary(GameInfo.Strings.ribbons);
	        
		ParseSettings.ChangeLocalizationStrings(GameInfo.Strings.movelist, GameInfo.Strings.specieslist);
	}
}

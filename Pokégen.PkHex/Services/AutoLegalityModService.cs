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

	public ITrainerInfo GetTrainerInfo<T>() where T : PKM, new()
	{
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
		APILegality.SetAllLegalRibbons = true;
		APILegality.SetMatchingBalls = true;
		APILegality.ForceSpecifiedBall = true;
		APILegality.UseXOROSHIRO = true;
		APILegality.AllowTrainerOverride = true;
		APILegality.AllowBatchCommands = true;
		Legalizer.EnableEasterEggs = false;
	}

	private static void InitializeTrainerDatabase()
	{
		var ot = GetEnvOrThrow("PKHEX_DEFAULT_OT");
		var trainerId = int.Parse(GetEnvOrThrow("PKHEX_DEFAULT_TID"));
		var secretId = int.Parse(GetEnvOrThrow("PKHEX_DEFAULT_SID"));
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
		PKMConverter.SetPrimaryTrainer(trainer);
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

using PKHeX.Core;
using Pokégen.PkHex.Models.Responses;

namespace Pokégen.PkHex.Extensions;

/// <summary>
/// Extension methods for the <see cref="PKM"/> class
/// </summary>
public static class PkmExtensions
{
	/// <summary>
	/// Checks if a <see cref="PKM"/> is legal by doing a <see cref="LegalityAnalysis"/> and checking if it can be traded
	/// </summary>
	/// <param name="pkm">The <see cref="PKM"/> to check legality for</param>
	/// <returns>bool indicating if its legal or not</returns>
	public static bool IsLegal(this PKM pkm) 
		=> new LegalityAnalysis(pkm).Valid && pkm.CanBeTraded();

	/// <summary>
	/// Helper extension converting a <see cref="PKM"/> to a <see cref="PokemonSummary"/>
	/// </summary>
	/// <param name="pkm">The Pokemon to convert</param>
	/// <returns></returns>
	public static PokemonSummary ToSummary(this PKM pkm)
		=> new()
		{
			Species = ((Species) pkm.Species).ToString(),
			DexNumber = pkm.Species,
			Nickname = pkm.Nickname,
			HeldItem = pkm.HeldItem.ToString(),
			Gender = ((Gender)pkm.Gender).ToString(),
			Nature = ((Nature)pkm.Nature).ToString(),
			StatNature = pkm.StatNature,
			Ability = ((Ability)pkm.Ability).ToString(),
			CurrentFriendship = pkm.CurrentFriendship,
			Form = pkm.Form,
			IsEgg = pkm.IsEgg,
			IsNicknamed = pkm.IsNicknamed,
			EXP = (int)pkm.EXP,
			TID = pkm.TID,
			OTName = pkm.OT_Name,
			OTGender = ((Gender)pkm.OT_Gender).ToString(),
			Ball = ((Ball)pkm.Ball).ToString(),
			MetLevel = pkm.Met_Level,
			Move1 = ((Move)pkm.Move1).ToString(),
			Move2 = ((Move)pkm.Move2).ToString(),
			Move3 = ((Move)pkm.Move3).ToString(),
			Move4 = ((Move)pkm.Move4).ToString(),
			Move1PP = pkm.Move1_PP,
			Move2PP = pkm.Move2_PP,
			Move3PP = pkm.Move3_PP,
			Move4PP = pkm.Move4_PP,
			Move1PPUps = pkm.Move1_PPUps,
			Move2PPUps = pkm.Move2_PPUps,
			Move3PPUps = pkm.Move3_PPUps,
			Move4PPUps = pkm.Move4_PPUps,
			EVHP = pkm.EV_HP,
			EVAttack = pkm.EV_ATK,
			EVDefense = pkm.EV_DEF,
			EVSpeed = pkm.EV_SPE,
			EVSpecialAttack = pkm.EV_SPA,
			EVSpecialDefense = pkm.EV_SPD,
			IVHP = pkm.IV_HP,
			IVAttack = pkm.IV_ATK,
			IVDefense = pkm.IV_DEF,
			IVSpeed = pkm.IV_SPE,
			IVSpecialAttack = pkm.IV_SPA,
			IVSpecialDefense = pkm.IV_SPD,
			StatusCondition = pkm.Status_Condition,
			StatLevel = pkm.Stat_Level,
			StatHPMax = pkm.Stat_HPMax,
			StatHPCurrent = pkm.Stat_HPCurrent,
			StatATK = pkm.Stat_ATK,
			StatDEF = pkm.Stat_DEF,
			StatSPE = pkm.Stat_SPE,
			StatSPA = pkm.Stat_SPA,
			StatSPD = pkm.Stat_SPD,
			Version = pkm.Version,
			SID = pkm.SID,
			PKRSStrain = pkm.PKRS_Strain,
			PKRSDays = pkm.PKRS_Days,
			EncryptionConstant = (int)pkm.EncryptionConstant,
			PID = 0,
			Language = ((LanguageID)pkm.Language).ToString(),
			FatefulEncounter = pkm.FatefulEncounter,
			TSV = pkm.TSV,
			PSV = pkm.PSV,
			Characteristic = pkm.Characteristic,
			MarkValue = pkm.MarkValue,
			MetLocation = pkm.Met_Location,
			EggLocation = pkm.Egg_Location,
			OTFriendship = pkm.OT_Friendship
		};
	
	private static bool CanBeTraded(this PKM pkm) 
		=> !FormInfo.IsFusedForm(pkm.Species, pkm.Form, pkm.Format);
}

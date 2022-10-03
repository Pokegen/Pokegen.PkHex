// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pokégen.PkHex.Models.Responses;

/// <summary>
/// A Summary of data from a Pokemon
/// </summary>
public record PokemonSummary
{
	/// <summary>
	/// Species of the pokemon
	/// </summary>
	public string Species { get; init; } = null!;

	/// <summary>
	/// Dex number of the pokemon
	/// </summary>
	public int DexNumber { get; init; }
	
	/// <summary>
	/// Nickname of the pokemon
	/// </summary>
	public string Nickname { get; init; } = null!;
	
	/// <summary>
	/// Item this pokemon held
	/// </summary>
	public string HeldItem { get; init; } = null!;
	
	/// <summary>
	/// Gender of the pokemon or "Genderless"
	/// </summary>
	public string Gender { get; init; } = null!;
	
	/// <summary>
	/// Nature of the pokemon
	/// </summary>
	public string Nature { get; init; } = null!;
	
	/// <summary>
	/// StatNature of the pokemon
	/// </summary>
	public int StatNature { get; init; }
	
	/// <summary>
	/// Ability of the pokemon
	/// </summary>
	public string Ability { get; init; } = null!;
	
	/// <summary>
	/// Current Friendship of the pokemon
	/// </summary>
	public int CurrentFriendship { get; init; }
	
	/// <summary>
	/// Form of the Pokemon
	/// </summary>
	public int Form { get; init; }
	
	/// <summary>
	/// If the pokemon is an egg
	/// </summary>
	public bool IsEgg { get; init; }
	
	/// <summary>
	/// If the pokemon has a nickname
	/// </summary>
	public bool IsNicknamed { get; init; }
	
	/// <summary>
	/// Experience of this pokemon
	/// </summary>
	public int EXP { get; init; } 
	
	/// <summary>
	/// Trainer id from the OT of this pokemon
	/// </summary>
	public int TID { get; init; }
	
	/// <summary>
	/// Name from the OT of this pokemon
	/// </summary>
	public string OTName { get; init; } = null!;

	/// <summary>
	/// Gender from the OT of this pokemon
	/// </summary>
	public string OTGender { get; init; } = null!;
	
	/// <summary>
	/// Ball this pokemon is in
	/// </summary>
	public string Ball { get; init; } = null!;
	
	/// <summary>
	/// Level this pokemon was met
	/// </summary>
	public int MetLevel { get; init; }
	
	/// <summary>
	/// First move of this pokemon
	/// </summary>
	public string Move1 { get; init; } = null!;
	
	/// <summary>
	/// Second move of this pokemon
	/// </summary>
	public string Move2 { get; init; } = null!;
	
	/// <summary>
	/// Third move of this pokemon
	/// </summary>
	public string Move3 { get; init; } = null!;
	
	/// <summary>
	/// Fourth move of this pokemon
	/// </summary>
	public string Move4 { get; init; } = null!;
	
	/// <summary>
	/// Amount of PP for the first move of this pokemon
	/// </summary>
	public int Move1PP { get; init; } 
	
	/// <summary>
	/// Amount of PP for the second move of this pokemon
	/// </summary>
	public int Move2PP { get; init; }
	
	/// <summary>
	/// Amount of PP for the third move of this pokemon
	/// </summary>
	public int Move3PP { get; init; }
	
	/// <summary>
	/// Amount of PP for the fourth move of this pokemon
	/// </summary>
	public int Move4PP { get; init; }
	
	/// <summary>
	/// Amount of PP Ups used on the first move of this pokemon
	/// </summary>
	public int Move1PPUps { get; init; }
	
	/// <summary>
	/// Amount of PP Ups used on the second move of this pokemon
	/// </summary>
	public int Move2PPUps { get; init; }
	
	/// <summary>
	/// Amount of PP Ups used on the third move of this pokemon
	/// </summary>
	public int Move3PPUps { get; init; }
	
	/// <summary>
	/// Amount of PP Ups used on the fourth move of this pokemon
	/// </summary>
	public int Move4PPUps { get; init; }
	
	/// <summary>
	/// Amount amount for HP of this Pokemon
	/// </summary>
	public int EVHP { get; init; }
	
	/// <summary>
	/// EVs amount for Attack of this Pokemon
	/// </summary>
	public int EVAttack { get; init; }
	
	/// <summary>
	/// EVs amount for Defense of this Pokemon
	/// </summary>
	public int EVDefense { get; init; }
	
	/// <summary>
	/// EVs amount for Speed of this Pokemon
	/// </summary>
	public int EVSpeed { get; init; }
	
	/// <summary>
	/// EVs amount for Special Attack of this Pokemon
	/// </summary>
	public int EVSpecialAttack { get; init; }
	
	/// <summary>
	/// EVs amount for Special Defense of this Pokemon
	/// </summary>
	public int EVSpecialDefense { get; init; }
	
	/// <summary>
	/// IVs for the HP of this Pokemon
	/// </summary>
	public int IVHP { get; init; }
	
	/// <summary>
	/// IVs for the Attack of this Pokemon
	/// </summary>
	public int IVAttack { get; init; }
	
	/// <summary>
	/// IVs for the Defense of this Pokemon
	/// </summary>
	public int IVDefense { get; init; }
	/// <summary>
	/// IVs for the Speed of this Pokemon
	/// </summary>
	public int IVSpeed { get; init; }
	
	/// <summary>
	/// IVs for the Special Attack of this Pokemon
	/// </summary>
	public int IVSpecialAttack { get; init; }
	
	/// <summary>
	/// IVs for the Special Defense of this Pokemon
	/// </summary>
	public int IVSpecialDefense { get; init; }
	
	/// <summary>
	/// current Status Condition of the Pokemon
	/// </summary>
	public int StatusCondition { get; init; }
	
	/// <summary>
	/// Level of the Pokemon
	/// </summary>
	public int StatLevel { get; init; }
	
	/// <summary>
	/// max HP this pokemon can have
	/// </summary>
	public int StatHPMax { get; init; }
	
	/// <summary>
	/// HP this pokemon currently has
	/// </summary>
	public int StatHPCurrent { get; init; }
	
	/// <summary>
	/// Attack this pokemon can have
	/// </summary>
	public int StatATK { get; init; }
	
	/// <summary>
	/// Attack this pokemon can have
	/// </summary>
	public int StatDEF { get; init; }
	
	/// <summary>
	/// Attack this pokemon can have
	/// </summary>
	public int StatSPE { get; init; }
	
	/// <summary>
	/// Attack this pokemon can have
	/// </summary>
	public int StatSPA { get; init; }
	
	/// <summary>
	/// Attack this pokemon can have
	/// </summary>
	public int StatSPD { get; init; }
	
	/// <summary>
	/// Version of this pokemon
	/// </summary>
	public int Version { get; init; }
	
	/// <summary>
	/// Secret ID from the OT of this Pokemon
	/// </summary>
	public int SID { get; init; }
	
	/// <summary>
	/// Pokerus strain of this Pokemon
	/// </summary>
	public int PKRSStrain { get; init; }
	
	/// <summary>
	/// How long this Pokemon had Pokerus
	/// </summary>
	public int PKRSDays { get; init; }
	
	/// <summary>
	/// Encryption constant for this Pokemon
	/// </summary>
	public int EncryptionConstant { get; init; }
	
	/// <summary>
	/// PID for this Pokemon
	/// </summary>
	public int PID { get; init; }
	
	/// <summary>
	/// Language of the game this Pokemon was caught in
	/// </summary>
	public string Language { get; init; } = null!;
	
	/// <summary>
	/// If this Pokemon was a fateful encounter
	/// </summary>
	public bool FatefulEncounter { get; init; }
	
	/// <summary>
	/// TSV of this pokemon
	/// </summary>
	public int TSV { get; init; }
	
	/// <summary>
	/// PSV of this pokemon
	/// </summary>
	public int PSV { get; init; }
	
	/// <summary>
	/// Characteristic of this pokemon
	/// </summary>
	public int Characteristic { get; init; }
	
	/// <summary>
	/// Mark value of this pokemon
	/// </summary>
	public int MarkValue { get; init; }
	
	/// <summary>
	/// Where this pokemon was met when caught
	/// </summary>
	public int MetLocation { get; init; }
	
	/// <summary>
	/// Location this egg was bred
	/// </summary>
	public int EggLocation { get; init; }
	
	/// <summary>
	/// Friendship level with the OT
	/// </summary>
	public int OTFriendship { get; init; }
}

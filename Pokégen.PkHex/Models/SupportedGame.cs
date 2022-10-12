namespace Pokégen.PkHex.Models;

/// <summary>
/// Supported Pokemon games of this Service
/// </summary>
public enum SupportedGame
{
	/// <summary>
	/// Pokemon Red, Blue and Yellow
	/// </summary>
	RBY,

	/// <summary>
	/// Pokemon Gold, Silver and Crystal
	/// </summary>
	GSC,

	/// <summary>
	/// Pokemon Ruby and Sapphire
	/// </summary>
	RS,
	
	/// <summary>
	/// Pokemon Fire Red and Leaf Green
	/// </summary>
	FRLG,
	
	/// <summary>
	/// Pokemon Emerald
	/// </summary>
	E,
	
	/// <summary>
	/// Pokemon Diamond and Pearl
	/// </summary>
	DP,
	
	/// <summary>
	/// Pokemon Platinum
	/// </summary>
	PT,
	
	/// <summary>
	/// Pokemon HeartGold and SoulSilver
	/// </summary>
	HGSS,

	/// <summary>
	/// Pokemon Black and White
	/// </summary>
	BW,
	
	/// <summary>
	/// Pokemon Black 2 and White 2
	/// </summary>
	BW2,

	/// <summary>
	/// Pokemon X and Y
	/// </summary>
	XY,
	
	/// <summary>
	/// Pokemon Omega Ruby and Alpha Sapphire
	/// </summary>
	ORAS,
	
	/// <summary>
	/// Pokemon Sun and Moon
	/// </summary>
	SM,
	
	/// <summary>
	/// Pokemon Ultra Sun and Ultra Moon
	/// </summary>
	USUM,
	
	/// <summary>
	/// Pokemon Let's Go Pikachu and Eevee
	/// </summary>
	LGPE,
	
	/// <summary>
	/// Pokemon Sword and Shield
	/// </summary>
	SWSH,

	/// <summary>
	/// Pokemon Brilliant Diamond and Shining Pearl
	/// </summary>
	BDSP,
	
	/// <summary>
	/// Pokemon Legends Arceus
	/// </summary>
	LA,
	
	/// <summary>
	/// Pokemon Scarlet and Violet
	/// </summary>
	[Unreleased]
	SV
}

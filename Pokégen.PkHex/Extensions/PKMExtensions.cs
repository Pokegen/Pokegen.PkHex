using PKHeX.Core;

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
	
	private static bool CanBeTraded(this PKM pkm) 
		=> !FormInfo.IsFusedForm(pkm.Species, pkm.Form, pkm.Format);
}

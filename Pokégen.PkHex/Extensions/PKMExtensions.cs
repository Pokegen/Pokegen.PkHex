using PKHeX.Core;
using Pokégen.PkHex.Services;

namespace Pokégen.PkHex.Extensions;

public static class PkmExtensions
{
	public static bool IsLegal(this PKM pkm) 
		=> new LegalityAnalysis(pkm).Valid && pkm.CanBeTraded();
	
	private static bool CanBeTraded(this PKM pkm) => !FormInfo.IsFusedForm(pkm.Species, pkm.Form, pkm.Format);
}

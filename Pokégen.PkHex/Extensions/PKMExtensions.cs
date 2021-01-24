using PKHeX.Core;

namespace Pokégen.PkHex.Extensions
{
	public static class PkmExtensions
	{
		public static bool IsLegal(this PKM pkm) 
			=> pkm is PK8 && new LegalityAnalysis(pkm).Valid;
	}
}

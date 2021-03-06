using PKHeX.Core;
using PKHeX.Core.AutoMod;

namespace Pokégen.PkHex.Extensions
{
	public static class ShowdownSetExtensions
	{
		public static IBattleTemplate GetTemplate(this ShowdownSet set) 
			=> new RegenTemplate(set);
	}
}

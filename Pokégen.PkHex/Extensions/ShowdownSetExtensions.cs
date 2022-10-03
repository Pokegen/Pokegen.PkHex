using PKHeX.Core;
using PKHeX.Core.AutoMod;

namespace Pokégen.PkHex.Extensions;

/// <summary>
/// Extensions for the <see cref="ShowdownSet"/> class
/// </summary>
public static class ShowdownSetExtensions
{
	/// <summary>
	/// Creates a new <see cref="IBattleTemplate"/> from this <see cref="ShowdownSet"/>
	/// </summary>
	/// <param name="set">The <see cref="ShowdownSet"/> to convert</param>
	/// <returns>The <see cref="IBattleTemplate"/></returns>
	public static IBattleTemplate GetTemplate(this ShowdownSet set) 
		=> new RegenTemplate(set);
}

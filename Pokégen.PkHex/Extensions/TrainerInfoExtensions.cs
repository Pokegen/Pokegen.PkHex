using PKHeX.Core;
using PKHeX.Core.AutoMod;

namespace Pokégen.PkHex.Extensions;

/// <summary>
/// Extension methods for the <see cref="ITrainerInfo"/> interface
/// </summary>
public static class TrainerInfoExtensions
{
	/// <summary>
	/// Checks if a <see cref="IBattleTemplate"/> is legal on this <see cref="ITrainerInfo"/> savefile
	/// </summary>
	/// <param name="sav">The <see cref="ITrainerInfo"/> to check this on</param>
	/// <param name="set">The <see cref="IBattleTemplate"/> to check for</param>
	/// <returns></returns>
	public static PKM GetLegal(this ITrainerInfo sav, IBattleTemplate set)
	{
		var result = sav.GetLegalFromSet(set, out _);
		return result;
	}
}

using System;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Util;

/// <summary>
/// <see cref="SupportedGame"/> util methods
/// </summary>
public static class SupportGameUtil
{
	/// <summary>
	/// Gets a <see cref="SupportedGame"/> value from a string
	/// </summary>
	/// <param name="game">The string representation</param>
	/// <returns>The <see cref="SupportedGame"/> representation of the string</returns>
	/// <exception cref="OutOfRangeException">If the string doesn't match a SupportedGame value or is not yet supported</exception>
	public static SupportedGame GetFromString(string game)
	{
		try
		{
			return EnumUtil.GetEnumValue<SupportedGame>(game);
		}
		catch (ArgumentOutOfRangeException)
		{
			throw new OutOfRangeException("Game is not a supported game!");
		}
	}
}

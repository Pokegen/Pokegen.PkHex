using System;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Util;

public static class SupportGameUtil
{
	public static SupportedGame GetFromString(string game)
	{
		try
		{
			return Enum.Parse<SupportedGame>(game, true);
		}
		catch (ArgumentException)
		{
			throw new OutOfRangeException("Game is not a supported game!");
		}
	}
}

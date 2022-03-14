using System;
using PKHeX.API.Exceptions;
using PKHeX.API.Models;

namespace PKHeX.API.Util;

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
			throw new BadRequestException("Game is not a supported game!");
		}
	}
}

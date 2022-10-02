using System;

namespace Pokégen.PkHex.Util;

public static class EnumUtil
{
	public static T GetEnumValue<T>(string value) where T : struct
	{
		try
		{
			return Enum.Parse<T>(value, true);
		}
		catch (ArgumentException)
		{
			throw new ArgumentOutOfRangeException(nameof(value), value, null);
		}
	}
}

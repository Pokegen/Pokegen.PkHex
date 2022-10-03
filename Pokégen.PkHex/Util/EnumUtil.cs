using System;

namespace Pokégen.PkHex.Util;

/// <summary>
/// Util methods for Enum related operations
/// </summary>
public static class EnumUtil
{
	/// <summary>
	/// Returns value as Enum Value
	/// </summary>
	/// <param name="value">The value which should be converted to enum value</param>
	/// <typeparam name="T">The enum to convert the value for</typeparam>
	/// <returns>The value represented as Enum value</returns>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
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

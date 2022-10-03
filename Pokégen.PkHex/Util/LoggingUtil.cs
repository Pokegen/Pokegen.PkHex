using System;

namespace Pokégen.PkHex.Util;

/// <summary>
/// Logging related util methods
/// </summary>
public static class LoggingUtil
{
	/// <summary>
	/// Checks if the current environment is development
	/// </summary>
	public static bool IsDevelopment
		=> Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
}

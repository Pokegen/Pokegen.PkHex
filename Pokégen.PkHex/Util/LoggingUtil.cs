using System;

namespace Pokégen.PkHex.Util
{
	public static class LoggingUtil
	{
		public static bool IsDevelopment
			=> Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
	}
}

using System;

namespace Pokégen.PkHex.Util
{
	public static class LoggingUtil
	{
		public static bool IsDevelopment
			=> Environment.GetEnvironmentVariable("ENVIRONMENT") == "DEVELOPMENT";
	}
}
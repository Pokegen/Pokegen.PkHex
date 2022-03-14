using System;

namespace PKHeX.API.Util;

public static class LoggingUtil
{
	public static bool IsDevelopment
		=> Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
}
using System;

namespace Pokégen.PkHex.Util;

public static class Util
{
	public static string GetEnvOrThrow(string env)
		=> Environment.GetEnvironmentVariable(env) ??
		   throw new Exception($"Missing environment variable \"{env}\"");
}
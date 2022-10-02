using System;

namespace Pokégen.PkHex.Util;

public static class Util
{
	public static string GetEnvOrThrow(string env)
		=> Environment.GetEnvironmentVariable(env) ??
		   throw new Exception($"Missing environment variable \"{env}\"");

	public static int GetEnvAsIntOrThrow(string env)
		=> int.Parse(GetEnvOrThrow(env));

	public static bool GetEnvAsBoolOrDefault(string env, bool @default)
	{
		var envVal = Environment.GetEnvironmentVariable(env);
		return envVal != null ? bool.Parse(envVal) : @default;
	}

	public static int GetEnvAsIntOrDefault(string env, int @default)
	{
		var envVal = Environment.GetEnvironmentVariable(env);
		return envVal != null ? int.Parse(envVal) : @default;
	}

	public static T GetEnvAsEnumOrDefault<T>(string env, T @default) where T : struct
	{
		var envVal = Environment.GetEnvironmentVariable(env);
		return envVal != null ? EnumUtil.GetEnumValue<T>(envVal) : @default;
	}
}

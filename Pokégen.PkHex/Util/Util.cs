using System;

namespace Pokégen.PkHex.Util;

/// <summary>
/// Util methods
/// </summary>
public static class Util
{
	/// <summary>
	/// Gets an environment variable or throws an exception if it's missing
	/// </summary>
	/// <param name="env">the env variable to retrieve</param>
	/// <returns>the value of the environment variable</returns>
	/// <exception cref="Exception">If the env variable is missing</exception>
	public static string GetEnvOrThrow(string env)
		=> Environment.GetEnvironmentVariable(env) ??
		   throw new Exception($"Missing environment variable \"{env}\"");

	/// <summary>
	/// Gets an environment variable and converts it to an integer or throws an exception if it's missing
	/// </summary>
	/// <param name="env">the env variable to retrieve</param>
	/// <returns>the value of the environment variable as integer</returns>
	/// <exception cref="Exception">If the env variable is missing</exception>
	public static int GetEnvAsIntOrThrow(string env)
		=> int.Parse(GetEnvOrThrow(env));

	/// <summary>
	/// Gets an environment variable and converts it to an boolean or uses a provided default value
	/// </summary>
	/// <param name="env">the env variable to retrieve</param>
	/// <param name="default">the default value to fallback if environment variable is missing</param>
	/// <returns>the value of the environment variable as boolean</returns>
	public static bool GetEnvAsBoolOrDefault(string env, bool @default)
	{
		var envVal = Environment.GetEnvironmentVariable(env);
		return envVal != null ? bool.Parse(envVal) : @default;
	}

	/// <summary>
	/// Gets an environment variable and converts it to an integer or uses a provided default value
	/// </summary>
	/// <param name="env">the env variable to retrieve</param>
	/// <param name="default">the default value to fallback if environment variable is missing</param>
	/// <returns>the value of the environment variable as integer</returns>
	public static int GetEnvAsIntOrDefault(string env, int @default)
	{
		var envVal = Environment.GetEnvironmentVariable(env);
		return envVal != null ? int.Parse(envVal) : @default;
	}

	/// <summary>
	/// Gets an environment variable and converts it to an enum or uses a provided default value
	/// </summary>
	/// <param name="env">the env variable to retrieve</param>
	/// <param name="default">the default value to fallback if environment variable is missing</param>
	/// <returns>the value of the environment variable as enum</returns>
	public static T GetEnvAsEnumOrDefault<T>(string env, T @default) where T : struct
	{
		var envVal = Environment.GetEnvironmentVariable(env);
		return envVal != null ? EnumUtil.GetEnumValue<T>(envVal) : @default;
	}
}

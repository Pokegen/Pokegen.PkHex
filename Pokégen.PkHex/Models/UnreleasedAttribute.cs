using System;

namespace Pokégen.PkHex.Models;

/// <summary>
/// Attribute for Unreleased Games so the api doesn't expose them in the api
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class UnreleasedAttribute : Attribute
{
	
}

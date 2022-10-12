using System;
using PKHeX.Core;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Util;

public static class SerebiiUtil
{
	public static string NormalizePokemonNames(string input)
		=> input
			.Trim()
			.Replace("'", "")
			.Replace("♀", "F")
			.Replace("♂", "M")
			.Replace(".", "")
			.Replace(" ", "")
			.Replace("-", "")
			.Replace("(", "")
			.Replace(")", "");

	public static string NormalizeAttackNames(string input)
		=> FilterAttackEdgeCases(input
			.Replace(" ", "")
			.Replace("-", ""));

	public static string GetUrlFromGenerationAndSpecies(int generation, Species species) 
		=> generation switch {
			1 => $"https://www.serebii.net/pokedex/{(int)species:000}.shtml",
			2 => $"https://www.serebii.net/pokedex-gs/{(int)species:000}.shtml",
			3 => $"https://www.serebii.net/pokedex-rs/{(int)species:000}.shtml",
			_ => throw new ArgumentOutOfRangeException(nameof(generation), generation, null)
		};

	private static string FilterAttackEdgeCases(string input)
		=> input
			.Replace("ViceGrip", "ViseGrip")
			.Replace("HiJumpKick", "HighJumpKick")
			.Replace("FaintAttack", "FeintAttack");
}

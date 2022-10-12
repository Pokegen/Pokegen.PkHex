using System;
using System.Collections.Generic;
using PKHeX.Core;

namespace Pokégen.PkHex.Models;

public record BasicPokemonInfo
{
	public Species Species { get; init; }

	public int DexNumber
		=> (int)Species;

	public IEnumerable<Ability> Abilities { get; init; } = Array.Empty<Ability>();
	public IEnumerable<Move> Moves { get; init; } = Array.Empty<Move>();
	public IEnumerable<string> Images { get; init; } = Array.Empty<string>();
	
	public IEnumerable<string> ShinyImages { get; set; } = Array.Empty<string>();
};

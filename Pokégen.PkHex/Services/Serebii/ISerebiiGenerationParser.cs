using System.Collections.Generic;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Services.Serebii;

public interface ISerebiiGenerationParser
{
	int Generation { get; }

	IEnumerable<BasicPokemonGameInfo> ParseHtmlToPokemons(string html);
}

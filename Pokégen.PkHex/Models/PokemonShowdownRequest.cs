using System.ComponentModel.DataAnnotations;

namespace Pokégen.PkHex.Models
{
	/// <summary>
	/// Request containing Pokemon Showdown set
	/// </summary>
	public record PokemonShowdownRequest([Required] string ShowdownSet);
}

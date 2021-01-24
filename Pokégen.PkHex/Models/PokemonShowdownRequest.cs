using System.ComponentModel.DataAnnotations;

namespace Pokégen.PkHex.Models
{
	/// <summary>
	/// Request containing Pokemon Showdown set
	/// </summary>
	public class PokemonShowdownRequest
	{
		/// <summary>
		/// Pokemon Showdown set
		/// </summary>
		[Required]
		public string ShowdownSet { get; set; }
	}
}

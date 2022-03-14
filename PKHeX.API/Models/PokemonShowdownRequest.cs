using System.ComponentModel.DataAnnotations;

namespace PKHeX.API.Models;

/// <summary>
/// Request containing Pokemon Showdown set
/// </summary>
public record PokemonShowdownRequest([Required] string ShowdownSet);
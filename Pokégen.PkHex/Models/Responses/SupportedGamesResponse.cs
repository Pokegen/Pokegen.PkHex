using System.Collections.Generic;

namespace Pokégen.PkHex.Models.Responses;

/// <summary>
/// Response for SupportedGames
/// </summary>
/// <param name="SupportedGames">List of Supported Games</param>
public record SupportedGamesResponse(IEnumerable<string> SupportedGames);

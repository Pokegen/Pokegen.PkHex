using System.Collections.Generic;

namespace Pokégen.PkHex.Models.Responses;

public record SupportedGamesResponse(IEnumerable<string> SupportedGames);

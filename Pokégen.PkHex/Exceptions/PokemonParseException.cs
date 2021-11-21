using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

public class PokemonParseException : BadRequestApiException<PokemonParseException>
{
	public PokemonParseException(string? message) : base(message)
	{
	}
}

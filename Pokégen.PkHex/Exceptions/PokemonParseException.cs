using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

public class PokemonParseException : BadRequestApiException
{
	public PokemonParseException(string? message) : base(message)
	{
	}
}

using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

/// <summary>
/// Exception regarding Pokemon parsing
/// </summary>
public class PokemonParseException : BadRequestApiException
{
	/// <summary>
	/// Creates a new instance with an optional message
	/// </summary>
	/// <param name="message">message describing the exception</param>
	public PokemonParseException(string? message) : base(message)
	{
	}
}

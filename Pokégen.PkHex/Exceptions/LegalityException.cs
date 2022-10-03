using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

/// <summary>
/// Exception regarding Legality
/// </summary>
public class LegalityException : BadRequestApiException
{
	/// <summary>
	/// Creates a new instance with an optional message
	/// </summary>
	/// <param name="message">message describing the exception</param>
	public LegalityException(string? message) : base(message)
	{
	}
}

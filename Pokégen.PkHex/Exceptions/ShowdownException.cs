using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

/// <summary>
/// Exception regarding a Showdown set
/// </summary>
public class ShowdownException : BadRequestApiException 
{
	/// <summary>
	/// Creates a new instance with an optional message
	/// </summary>
	/// <param name="message">message describing the exception</param>
	public ShowdownException(string? message) : base(message)
	{
	}
}

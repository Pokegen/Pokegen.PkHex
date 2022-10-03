using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

/// <summary>
/// Exception regards something being out of range/bounds
/// </summary>
public class OutOfRangeException : BadRequestApiException
{
	/// <summary>
	/// Creates a new instance with an optional message
	/// </summary>
	/// <param name="message">message describing the exception</param>
	public OutOfRangeException(string? message) : base(message)
	{
	}
}

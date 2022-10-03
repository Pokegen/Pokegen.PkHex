using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

/// <summary>
/// Exception regarding something nto being implemented
/// </summary>
public class NotImplementedException : BadRequestApiException
{
	/// <summary>
	/// Creates a new instance with an optional message
	/// </summary>
	/// <param name="message">message describing the exception</param>
	public NotImplementedException(string? message) : base(message) {}
}

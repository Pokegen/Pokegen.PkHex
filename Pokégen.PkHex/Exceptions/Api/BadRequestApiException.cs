using AspNetCore.ExceptionHandler.Attributes;
using Microsoft.AspNetCore.Http;

namespace Pokégen.PkHex.Exceptions.Api;

/// <summary>
/// 
/// </summary>
[StatusCode(StatusCodes.Status400BadRequest)]
public abstract class BadRequestApiException : ApiBaseException
{
	/// <summary>
	/// Creates a new instance with an optional message
	/// </summary>
	/// <param name="message">message describing the exception</param>
	protected BadRequestApiException(string? message) : base(message)
	{
	}
}

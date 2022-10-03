using System;
using AspNetCore.ExceptionHandler;

namespace Pokégen.PkHex.Exceptions.Api;

/// <summary>
/// Abstract class for api exceptions implementing 
/// </summary>
public abstract class ApiBaseException : Exception, IExplainableException
{
	/// <summary>
	/// Creates a new instance with a message
	/// </summary>
	/// <param name="message">message describing the exception</param>
	protected ApiBaseException(string? message) : base(message)
	{
	}

	/// <inheritdoc />
	public object Explain()
		=> new ApiExceptions(GetType().Name.Replace("Exception", ""), Message);
}

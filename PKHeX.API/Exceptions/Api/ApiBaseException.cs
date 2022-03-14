using System;
using AspNetCore.ExceptionHandler;

namespace PKHeX.API.Exceptions.Api;

public abstract class ApiBaseException : Exception, IExplainableException
{
	protected ApiBaseException()
	{
	}

	protected ApiBaseException(string? message) : base(message)
	{
	}

	public object Explain()
		=> new ApiExceptions(GetType().Name.Replace("Exception", ""), Message);
}
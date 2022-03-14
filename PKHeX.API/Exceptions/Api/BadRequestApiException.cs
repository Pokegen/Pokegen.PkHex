using AspNetCore.ExceptionHandler.Attributes;
using Microsoft.AspNetCore.Http;

namespace PKHeX.API.Exceptions.Api;

[StatusCode(StatusCodes.Status400BadRequest)]
public abstract class BadRequestApiException : ApiBaseException
{
	protected BadRequestApiException()
	{
	}

	protected BadRequestApiException(string? message) : base(message)
	{
	}
}
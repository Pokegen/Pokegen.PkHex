using AspNetCore.ExceptionHandler.Attributes;
using Microsoft.AspNetCore.Http;

namespace Pokégen.PkHex.Exceptions.Api
{
	[StatusCode(StatusCodes.Status400BadRequest)]
	public abstract class BadRequestApiException<T> : ApiBaseException<T>
	{
		protected BadRequestApiException()
		{
		}

		protected BadRequestApiException(string? message) : base(message)
		{
		}
	}
}

using PKHeX.API.Exceptions.Api;

namespace PKHeX.API.Exceptions;

public class BadRequestException : BadRequestApiException
{
	public BadRequestException(string? message) : base(message)
	{
	}
}

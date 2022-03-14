using PKHeX.API.Exceptions.Api;

namespace PKHeX.API.Exceptions;

public class NotImplementedException : BadRequestApiException
{
	public NotImplementedException(string? message) : base(message) {}
}
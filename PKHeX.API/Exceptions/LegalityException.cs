using PKHeX.API.Exceptions.Api;

namespace PKHeX.API.Exceptions;

public class LegalityException : BadRequestApiException
{
	public LegalityException()
	{
	}

	public LegalityException(string? message) : base(message)
	{
	}
}

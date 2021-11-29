using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

public class OutOfRangeException : BadRequestApiException
{
	public OutOfRangeException(string? message) : base(message)
	{
	}
}

using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

public class NotImplementedException : BadRequestApiException
{
	public NotImplementedException(string? message) : base(message) {}
}

using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions
{
	public class LegalityException : BadRequestApiException
	{
		public LegalityException()
		{
		}

		public LegalityException(string? message) : base(message)
		{
		}
	}
}

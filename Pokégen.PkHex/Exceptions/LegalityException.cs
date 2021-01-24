using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions
{
	public class LegalityException : BadRequestApiException<LegalityException>
	{
		public LegalityException()
		{
		}

		public LegalityException(string? message) : base(message)
		{
		}
	}
}

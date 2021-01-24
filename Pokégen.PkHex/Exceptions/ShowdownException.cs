using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions
{
	public class ShowdownException : BadRequestApiException<ShowdownException> 
	{
		public ShowdownException(string? message) : base(message)
		{
		}
	}
}

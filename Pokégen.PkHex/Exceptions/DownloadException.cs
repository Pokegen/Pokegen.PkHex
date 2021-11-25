using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions
{
	public class DownloadException : BadRequestApiException
	{
		public DownloadException(string? message) : base(message)
		{
		}
	}
}

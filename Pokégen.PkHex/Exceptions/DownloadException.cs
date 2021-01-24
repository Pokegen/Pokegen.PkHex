using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions
{
	public class DownloadException : BadRequestApiException<DownloadException>
	{
		public DownloadException(string? message) : base(message)
		{
		}
	}
}

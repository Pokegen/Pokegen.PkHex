using Pokégen.PkHex.Exceptions.Api;

namespace Pokégen.PkHex.Exceptions;

/// <summary>
/// Exception regarding Downloading
/// </summary>
public class DownloadException : BadRequestApiException
{
	/// <summary>
	/// Creates a new instance with an optional message
	/// </summary>
	/// <param name="message">message describing the exception</param>
	public DownloadException(string? message) : base(message)
	{
	}
}

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Services;

/// <summary>
/// Service providing Downloading Pokemon functionality
/// </summary>
public class DownloaderService
{
	private HttpClient HttpClient { get; }
	
	private PokemonService PokemonService { get; }

	/// <summary>
	/// Creates a new <see cref="DownloaderService"/> instance
	/// </summary>
	/// <param name="httpClient">The <see cref="HttpClient"/> to use</param>
	/// <param name="pokemonService">The <see cref="PokemonService"/> to use</param>
	public DownloaderService(HttpClient httpClient, PokemonService pokemonService)
	{
		HttpClient = httpClient;
		PokemonService = pokemonService;
	}
		
	/// <summary>
	/// Downloads a Pokemon from an URI
	/// </summary>
	/// <param name="uri">The uri to download the file from</param>
	/// <param name="length">Optionally the length if already known ahead of making a request</param>
	/// <param name="wantedGame">The game this pokemon file should be part of</param>
	/// <returns>The downloaded <see cref="PKM"/> file</returns>
	/// <exception cref="DownloadException">If the request fails or other errors are encountered (such as the file being invalid or having an invalid size)</exception>
	public async Task<PKM> DownloadPkmAsync(Uri uri, long? length, SupportedGame wantedGame)
	{
		long? size = null;
		if (length != null) size = (long) length;
		else
		{
			var request = new HttpRequestMessage
			{
				RequestUri = uri,
				Method = HttpMethod.Head
			};

			var response = await HttpClient.SendAsync(request);

			response.Headers.TryGetValues("content-length", out var values);

			var first = values?.First();

			if (first != null)
				size = long.Parse(first);
		}

		if (size != null)
		{
			if (!EntityDetection.IsSizePlausible((long) size))
				throw new DownloadException("Invalid size");
		}

		var fileName = Path.GetExtension($"{uri.Scheme}{Uri.SchemeDelimiter}{uri.Authority}{uri.AbsolutePath}");
			
		var buffer = await HttpClient.GetByteArrayAsync(uri);
			
		var entityContext = EntityFileExtension.GetContextFromExtension(fileName);
		var pkm = EntityFormat.GetFromBytes(buffer, entityContext);
		if (pkm == null)
			throw new DownloadException("Invalid pkm file");

		return PokemonService.ConvertToWantedType(pkm, wantedGame);
	}
}

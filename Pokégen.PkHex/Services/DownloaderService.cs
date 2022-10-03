using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Services;

public class DownloaderService
{
	private HttpClient HttpClient { get; }
	
	private PokemonService PokemonService { get; }

	public DownloaderService(HttpClient httpClient, PokemonService pokemonService)
	{
		HttpClient = httpClient;
		PokemonService = pokemonService;
	}
		
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

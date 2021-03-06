using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;

namespace Pokégen.PkHex.Services
{
	public class DownloaderService
	{
		private HttpClient HttpClient { get; }

		public DownloaderService(HttpClient httpClient)
			=> HttpClient = httpClient;
		
		public async Task<PKM> DownloadPkmAsync(Uri uri, long? length)
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
				if (!PKX.IsPKM((long) size))
					throw new DownloadException("Invalid size");
			}

			var fileName = Path.GetExtension($"{uri.Scheme}{Uri.SchemeDelimiter}{uri.Authority}{uri.AbsolutePath}");
			
			var buffer = await HttpClient.GetByteArrayAsync(uri);
			
			var pkm = PKMConverter.GetPKMfromBytes(buffer, fileName.Contains("pk6") ? 6 : 7);
			if (pkm == null)
				throw new DownloadException("Invalid pkm attachment");

			return PKMConverter.ConvertToType(pkm, typeof(PK8), out _) ?? pkm;
		}
	}
}

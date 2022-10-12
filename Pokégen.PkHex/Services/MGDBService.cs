using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokégen.PkHex.Services;

public class MGDBService
{
	private const string RepoUrl = "https://github.com/projectpokemon/EventsGallery/archive/master.zip";
	
	private HttpClient HttpClient { get; }

	public MGDBService(HttpClient httpClient) 
		=> HttpClient = httpClient;

	public async Task DownloadAndCleanup(string path)
	{
		if (Directory.Exists(path))
			Directory.Delete(path, true);
		
		const string tempPath = "temp.zip";
		
		await DownloadZip(tempPath);
		
		ExtractAndDeleteZip(tempPath, "./mgdb");
		
		var mgdbPath = System.IO.Path.Combine(path, "EventsGallery-master");
		string Path(string s) => System.IO.Path.Combine(mgdbPath, s);
		Directory.Delete(Path("Unreleased"), true);
		Directory.Delete(Path("Extras"), true);
		File.Delete(Path(".gitignore"));
		File.Delete(Path("README.md"));
	}

	private async Task DownloadZip(string path)
	{
		var res = await HttpClient.GetAsync(RepoUrl);

		await using var fs = new FileStream(path, FileMode.CreateNew);
		await res.Content.CopyToAsync(fs);
	}

	private static void ExtractAndDeleteZip(string path, string dest)
	{
		ZipFile.ExtractToDirectory(path, dest);
		File.Delete(path);
	}
}

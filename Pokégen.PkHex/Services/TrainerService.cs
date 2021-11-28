using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PKHeX.Core;

namespace Pokégen.PkHex.Services;

public class TrainerService
{
	public async Task<ITrainerInfo> GetTrainerInfo(IFormFile data, string game)
	{
		ITrainerInfo saveFile;
		
		await using var fileStream = new MemoryStream();
		await data.CopyToAsync(fileStream);
		var fileData = fileStream.ToArray();
		
		switch (game.ToLower())
		{
			case "swsh":
			{
				var sav8Swsh = new SAV8SWSH();
				fileData.CopyTo(sav8Swsh.MyStatus.Data);
				saveFile = sav8Swsh;
				break;
			}
			case "bdsp":
			{
				var bdspSave = new SAV8BS();
				fileData.CopyTo(bdspSave.MyStatus.Data);
				saveFile = bdspSave;
				break;
			}
			default:
				throw new Exceptions.NotImplementedException("Requested Game not implemented yet");
		}

		return saveFile;
	}
}

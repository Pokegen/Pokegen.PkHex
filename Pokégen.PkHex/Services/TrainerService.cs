using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Services;

public class TrainerService
{
	public async Task<ITrainerInfo> GetTrainerInfo(IFormFile data, SupportedGame game)
	{
		ITrainerInfo saveFile;
		
		await using var fileStream = new MemoryStream();
		await data.CopyToAsync(fileStream);
		var fileData = fileStream.ToArray();
		
		switch (game)
		{
			case SupportedGame.LGPE:
			{
				saveFile = new SAV7b();
				break;
			}
			case SupportedGame.SWSH:
			{
				var sav8Swsh = new SAV8SWSH();
				fileData.CopyTo(sav8Swsh.MyStatus.Data);
				saveFile = sav8Swsh;
				break;
			}
			case SupportedGame.BDSP:
			{
				var bdspSave = new SAV8BS();
				fileData.CopyTo(bdspSave.MyStatus.Data);
				saveFile = bdspSave;
				break;
			}
			case SupportedGame.PLA:
				var save = new SAV8LA();
				fileData.CopyTo(save.MyStatus.Data);
				saveFile = save;
				break;
			default:
				throw new NotImplementedException("Requested Game not implemented yet");
		}

		return saveFile;
	}
}

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
		ITrainerInfo trainerInfo;
		
		await using var fileStream = new MemoryStream();
		await data.CopyToAsync(fileStream);
		var fileData = fileStream.ToArray();
		
		switch (game)
		{
			case SupportedGame.RBY:
			{
				var save1Rby = new SAV1();
				fileData.CopyTo(save1Rby.Data);
				trainerInfo = save1Rby;
				break;
			}
			case SupportedGame.GSC:
			{
				var save2Gsc = new SAV2();
				fileData.CopyTo(save2Gsc.Data);
				trainerInfo = save2Gsc;
				break;
			}
			case SupportedGame.RS:
			{
				var save3Rs = new SAV3RS();
				fileData.CopyTo(save3Rs.Data);
				trainerInfo = save3Rs;
				break;
			}
			case SupportedGame.E:
			{
				var save3E = new SAV3E();
				fileData.CopyTo(save3E.Data);
				trainerInfo = save3E;
				break;
			}
			case SupportedGame.DP:
			{
				var save4Dpp = new SAV4DP();
				fileData.CopyTo(save4Dpp.Data);
				trainerInfo = save4Dpp;
				break;
			}
			case SupportedGame.PT:
			{
				var save4Pt = new SAV4Pt();
				fileData.CopyTo(save4Pt.Data);
				trainerInfo = save4Pt;
				break;
			}
			case SupportedGame.HGSS:
			{
				var save4Hgss = new SAV4HGSS();
				fileData.CopyTo(save4Hgss.Data);
				trainerInfo = save4Hgss;
				break;
			}
			case SupportedGame.BW:
			{
				var save5Bw = new SAV5BW();
				fileData.CopyTo(save5Bw.PlayerData.Data);
				trainerInfo = save5Bw;
				break;
			}
			case SupportedGame.BW2:
			{
				var save5Bw2 = new SAV5B2W2();
				fileData.CopyTo(save5Bw2.PlayerData.Data);
				trainerInfo = save5Bw2;
				break;
			}
			case SupportedGame.XY:
			{
				var save6Xy = new SAV6XY();
				fileData.CopyTo(save6Xy.Status.Data);
				trainerInfo = save6Xy;
				break;
			}
			case SupportedGame.ORAS:
			{
				var save6Oras = new SAV6AO();
				fileData.CopyTo(save6Oras.Status.Data);
				trainerInfo = save6Oras;
				break;
			}
			case SupportedGame.SM:
			{
				var save7Sm = new SAV7SM();
				fileData.CopyTo(save7Sm.MyStatus.Data);
				trainerInfo = save7Sm;
				break;
			}
			case SupportedGame.USUM:
			{
				var save7Usum = new SAV7USUM();
				fileData.CopyTo(save7Usum.MyStatus.Data);
				trainerInfo = save7Usum;
				break;
			}
			case SupportedGame.LGPE:
			{
				var save7Lpge = new SAV7b();
				fileData.CopyTo(save7Lpge.Status.Data);
				trainerInfo = save7Lpge;
				break;
			}
			case SupportedGame.SWSH:
			{
				var sav8Swsh = new SAV8SWSH();
				fileData.CopyTo(sav8Swsh.MyStatus.Data);
				trainerInfo = sav8Swsh;
				break;
			}
			case SupportedGame.BDSP:
			{
				var save8Bdsp = new SAV8BS();
				fileData.CopyTo(save8Bdsp.MyStatus.Data);
				trainerInfo = save8Bdsp;
				break;
			}
			case SupportedGame.PLA:
				var save8Pla = new SAV8LA();
				fileData.CopyTo(save8Pla.MyStatus.Data);
				trainerInfo = save8Pla;
				break;
			default:
				throw new NotImplementedException("Requested Game not implemented yet");
		}

		return trainerInfo;
	}
}

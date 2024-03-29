using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PKHeX.Core;
using Pokégen.PkHex.Exceptions;
using Pokégen.PkHex.Models;

namespace Pokégen.PkHex.Services;

/// <summary>
/// Service providing Trainer related functionality
/// </summary>
public class TrainerService
{
	/// <summary>
	/// Get <see cref="ITrainerInfo"/> from a save file
	/// </summary>
	/// <param name="data">For Gen 1 - 4 the complete save file data, for Gen 5-8 the Status/MyStatus block of the savefile</param>
	/// <param name="game">The <see cref="SupportedGame"/> this data is from</param>
	/// <returns>The <see cref="ITrainerInfo"/> of the save file block(s)</returns>
	/// <exception cref="NotImplementedException">If the requested <see cref="SupportedGame"/> is not yet supported</exception>
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
				var save1Rby = new SAV1(fileData);
				trainerInfo = save1Rby;
				break;
			}
			case SupportedGame.GSC:
			{
				var save2Gsc = new SAV2(fileData);
				trainerInfo = save2Gsc;
				break;
			}
			case SupportedGame.RS:
			{
				var save3Rs = new SAV3RS(fileData);
				trainerInfo = save3Rs;
				break;
			}
			case SupportedGame.E:
			{
				var save3E = new SAV3E(fileData);
				trainerInfo = save3E;
				break;
			}
			case SupportedGame.DP:
			{
				var save4Dpp = new SAV4DP(fileData);
				trainerInfo = save4Dpp;
				break;
			}
			case SupportedGame.PT:
			{
				var save4Pt = new SAV4Pt(fileData);
				trainerInfo = save4Pt;
				break;
			}
			case SupportedGame.HGSS:
			{
				var save4Hgss = new SAV4HGSS(fileData);
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
			case SupportedGame.LA:
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

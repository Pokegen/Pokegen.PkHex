using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pokégen.PkHex.Models;
using Pokégen.PkHex.Models.Responses;

namespace Pokégen.PkHex.Controllers;

[Route("/games")]
[ApiController]
public class GamesController : ControllerBase
{
	[HttpGet("supported")]
	public Task<IActionResult> GetSupportedGames()
	{
		var supportedGames = Enum.GetValues<SupportedGame>()
			.Where(supportedGame => supportedGame.GetType().GetMember(supportedGame.ToString()).FirstOrDefault()!.GetCustomAttribute<UnreleasedAttribute>() == null)
			.Select(game => game.ToString());

		return Task.FromResult(Ok(new SupportedGamesResponse(supportedGames)) as IActionResult);
	}
}

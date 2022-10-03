using System;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokégen.PkHex.Models;
using Pokégen.PkHex.Models.Responses;

namespace Pokégen.PkHex.Controllers;

/// <summary>
/// Controller which serves routes for pokemon game related data
/// </summary>
[Route("games")]
[ApiController]
public class GamesController : ControllerBase
{
	/// <summary>
	/// Gets all Pokemon games which are released and this api supports
	/// </summary>
	/// <returns>List of supported games</returns>
	/// <response code="200">Returns a list of all supported games</response>
	[HttpGet("supported")]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public Task<IActionResult> GetSupportedGames()
	{
		var supportedGames = Enum.GetValues<SupportedGame>()
			.Where(supportedGame => supportedGame.GetType().GetMember(supportedGame.ToString()).FirstOrDefault()!.GetCustomAttribute<UnreleasedAttribute>() == null)
			.Select(game => game.ToString());

		return Task.FromResult(Ok(new SupportedGamesResponse(supportedGames)) as IActionResult);
	}
}

namespace Pokégen.PkHex.Models;

/// <summary>
/// API payload containing trainer info
/// </summary>
/// <param name="TrainerId">Id of the trainer</param>
/// <param name="SecretId">Secret Id of trainer</param>
/// <param name="Ot">OT name of trainer</param>
/// <param name="Gender">Gender of trainer</param>
/// <param name="Game">Game of the trainer</param>
/// <param name="Language">Language of the trainer</param>
/// <param name="Generation">Generation of the trainer</param>
public record TrainerInfoResponse(int TrainerId, int SecretId, string Ot, int Gender, int Game, int Language, int Generation);

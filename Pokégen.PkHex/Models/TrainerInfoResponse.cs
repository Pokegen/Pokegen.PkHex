namespace Pokégen.PkHex.Models;

public record TrainerInfoResponse(int trainerId, int secretId, string OT, int gender, int game, int language, int generation);

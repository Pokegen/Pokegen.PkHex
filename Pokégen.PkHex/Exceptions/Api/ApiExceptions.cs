namespace Pokégen.PkHex.Exceptions.Api;

/// <summary>
/// payload containing Api Exception type & message
/// </summary>
/// <param name="ErrorType">The type of error</param>
/// <param name="Message">The message of the error</param>
public record ApiExceptions(string ErrorType, string Message);

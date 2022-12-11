namespace Shared;

/// <summary>
/// Stores shared names used in both client and hub
/// </summary>
public static class Messages
{
    /// <summary>
    /// Name of the Hub method to send a move
    /// </summary>
    public const string GUESSNUMBER = "GuessNumber";

    /// <summary>
    /// Name of the Hub method to register a new player
    /// </summary>
    public const string REGISTER = "Register";

    /// <summary>
    /// Event name when a game state update is received
    /// </summary>
    public const string UPDATEGAMESTATE = "UpdateGameState";
}
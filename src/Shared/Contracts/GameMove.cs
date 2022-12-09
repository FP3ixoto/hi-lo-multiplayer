namespace Shared.Contracts;

public sealed class GameMove
{
    public string Player { get; init; } = string.Empty;
    public int ProposedNumber { get; init; }
    /// <summary>
    /// The system response regarding the proposed number:
    /// -1 if the mystery number is lower
    /// 0 if the proposed number is the correct answer
    /// 1 if the mystery number is higher
    /// </summary>
    public int HiLo { get; init; }
}

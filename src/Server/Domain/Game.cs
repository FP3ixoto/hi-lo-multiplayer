using System.Security.Cryptography;

namespace Server.Domain
{
    public class Game
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Player? CurrentPlayer { get; private set; }
        public Player? Player1 { get; private set; }
        public Player? Player2 { get; private set; }
        public GameStatus State { get; private set; } = GameStatus.AwaitingPlayers;

        private readonly int _mysteryNumber = RandomNumberGenerator.GetInt32(99);

        public void AddPlayer(string name, string connectionId)
        {
            if (State != GameStatus.AwaitingPlayers)
            {
                throw new InvalidOperationException("Game already started.");
            }

            if (Player1 is null)
            {
                Player1 = new(name, connectionId);
            }
            else
            {
                Player2 = new(name, connectionId);
                Start();
            }
        }

        private void Start()
        {
            State = GameStatus.InProgress;
            CurrentPlayer = Player1; //TODO: make it random
        }

        private void FinishGame()
        {
            State = GameStatus.Finished;
            CurrentPlayer = null;
        }

        private void UpdatePlayerTurn() => CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns>
        /// The system response regarding the proposed number:
        /// -1 if the mystery number is lower
        /// 0 if the proposed number is the correct answer
        /// 1 if the mystery number is higher
        /// </returns>
        public int ProposeNumber(int number)
        {
            var isCorrect = _mysteryNumber == number;
            if (isCorrect)
            {
                FinishGame();
            }
            else
            {
                UpdatePlayerTurn();
            }

            return isCorrect ? 0 : _mysteryNumber < number ? -1 : 1;
        }

        public void AssertValidInProgressGameState()
        {
            if (Player1 is null || Player2 is null)
            {
                throw new InvalidOperationException("Not enough players.");
            }

            if (State != GameStatus.InProgress)
            {
                throw new InvalidOperationException("Players haven't started a game yet.");
            }
        }
    }
}

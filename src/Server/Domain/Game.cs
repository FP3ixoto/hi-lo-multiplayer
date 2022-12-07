using System.Security.Cryptography;

namespace Server.Domain
{
    public class Game
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Player? CurrentPlayer { get; private set; }
        public Player? Player1 { get; private set; }
        public Player? Player2 { get; private set; }
        public GameState State { get; private set; } = GameState.AwaitingPlayers;

        private readonly int _mysteryNumber = RandomNumberGenerator.GetInt32(99);

        //public Game()
        //{
        //    _mysteryNumber = RandomNumberGenerator.GetInt32(99);
        //}

        public void AddPlayer(string name, string connectionId)
        {
            if (State != GameState.AwaitingPlayers)
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
            State = GameState.InProgress;
            CurrentPlayer = Player1; //TODO: make it random
        }

        public void UpdatePlayerTurn() => CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;
    }
}

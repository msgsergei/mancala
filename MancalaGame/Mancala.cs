using System.Text;

namespace MancalaGame
{
    public class Mancala : IMancala
    {
        private readonly Dictionary<MancalaPlayer, MancalaPlayerState> _playerStates = new();
        private readonly List<MancalaStateUpdate> _lastMoveUpdates = new List<MancalaStateUpdate>(20);

        public bool IsFinished { get; private set; } = false;

        public MancalaPlayer Player { get; private set; } = MancalaPlayer.One;
        public IReadOnlyCollection<MancalaStateUpdate> LastMoveUpdates => _lastMoveUpdates;

        public Mancala()
            : this(MancalaDefaults.DefaultStoreValue,
                  MancalaDefaults.CreateDefaultPits(),
                  MancalaDefaults.DefaultStoreValue,
                  MancalaDefaults.CreateDefaultPits())
        {
        }

        internal Mancala(int player1store, int[] player1Pits, int player2store, int[] player2pits, MancalaPlayer startingPlayer = MancalaPlayer.One)
        {
            _playerStates.Add(MancalaPlayer.One,
                new MancalaPlayerState(player1store, player1Pits)
                {
                    StateUpdateHandler = x => OnMancalaPlayerStateUpdateHandle(MancalaPlayer.One, x)
                });
            _playerStates.Add(MancalaPlayer.Two,
                new MancalaPlayerState(player2store, player2pits)
                {
                    StateUpdateHandler = x => OnMancalaPlayerStateUpdateHandle(MancalaPlayer.Two, x)
                });

            Player = startingPlayer;
        }

        public MancalaPlayerState GetState(MancalaPlayer player)
        {
            return _playerStates[player];
        }

        public void NewGame()
        {
            GetState(MancalaPlayer.One).Reset();
            GetState(MancalaPlayer.Two).Reset();

            IsFinished = false;
            Player = MancalaPlayer.One;
        }

        public void MakeMove(MancalaPlayer activePlayer, int pitIndex)
        {
            EnsureMoveIsValid(activePlayer, pitIndex);

            _lastMoveUpdates.Clear();

            MancalaPlayer nextTurnPlayer = GetAnotherPlayer(activePlayer);

            var processingPlayer = activePlayer;
            var processingState = GetState(processingPlayer);

            var stonesInHand = processingState.TakeStonesFromPit(pitIndex++);

            while (stonesInHand > 0)
            {
                stonesInHand--;

                if (pitIndex >= processingState.Pits.Count)
                {
                    bool isActivePlayerOwnStore = activePlayer == processingPlayer;
                    if (isActivePlayerOwnStore)
                    {
                        processingState.PutStonesIntoStore(1);
                        if (stonesInHand == 0)
                        {
                            // When the last seed in your hand lands in your store, take another turn
                            nextTurnPlayer = activePlayer;
                            break;
                        }
                    }

                    processingPlayer = GetAnotherPlayer(processingPlayer);
                    processingState = GetState(processingPlayer);
                    pitIndex = 0;

                    if (isActivePlayerOwnStore)
                    {
                        continue;
                    }
                }

                processingState.PutOneStoneIntoPit(pitIndex);
                if (stonesInHand == 0 && activePlayer == processingPlayer && processingState.NumberOfStonesInPit(pitIndex) == 1)
                {
                    // When the last seed in your hand lands in one of your own pits, if that pit had been empty
                    // you get to keep all of the seeds in your opponents pit on the opposite side.
                    // Put those captured seeds,as well as the last seed that you just played on your side, into the store.
                    var stones = processingState.TakeStonesFromPit(pitIndex);

                    var anotherState = GetState(GetAnotherPlayer(processingPlayer));
                    var anotherOppositePitIndex = anotherState.Pits.Count - 1 - pitIndex;
                    var anotherStones = anotherState.TakeStonesFromPit(anotherOppositePitIndex);

                    processingState.PutStonesIntoStore(stones + anotherStones);
                }
                pitIndex++;
            }

            var activePlayerState = GetState(activePlayer);
            var opponentPlayerState = GetState(GetAnotherPlayer(activePlayer));
            if (activePlayerState.IsEmptyPits || opponentPlayerState.IsEmptyPits)
            {
                activePlayerState.MoveAllStonesFromPitsIntoStore();
                opponentPlayerState.MoveAllStonesFromPitsIntoStore();

                var playerOneScore = GetState(MancalaPlayer.One).Store;
                var playerTwoScore = GetState(MancalaPlayer.Two).Store;
                var winner = playerOneScore == playerTwoScore
                    ? activePlayer // tiebreaker
                    : playerOneScore > playerTwoScore ? MancalaPlayer.One : MancalaPlayer.Two;

                Player = winner;
                IsFinished = true;
            }
            else
            {
                Player = nextTurnPlayer;
            }
        }

        private void EnsureMoveIsValid(MancalaPlayer player, int pitIndex)
        {
            if (IsFinished)
            {
                throw new InvalidOperationException("Game is already finished");
            }

            if (player != Player)
            {
                throw new InvalidOperationException("Another player turn");
            }

            var playerState = GetState(player);

            if (pitIndex < 0 || pitIndex >= playerState.Pits.Count)
            {
                throw new IndexOutOfRangeException("PitIndex is out of range");
            }

            if (playerState.NumberOfStonesInPit(pitIndex) == 0)
            {
                throw new InvalidOperationException("Can not select empty pit");
            }
        }

        private MancalaPlayer GetAnotherPlayer(MancalaPlayer player)
        {
            return player == MancalaPlayer.One ? MancalaPlayer.Two : MancalaPlayer.One;
        }

        private void OnMancalaPlayerStateUpdateHandle(MancalaPlayer player, MancalaPlayerStateUpdate update)
        {
            _lastMoveUpdates.Add(new MancalaStateUpdate()
            {
                Change = update.Change,
                PitIndex = update.PitIndex,
                Player = player
            });
        }

        public override string ToString()
        {
            var state1 = GetState(MancalaPlayer.One);
            var state2 = GetState(MancalaPlayer.Two);

            StringBuilder sb = new();
            sb.Append($"{state2.Store} => {string.Join(" - ", state2.Pits.Reverse())}");
            sb.Append(" /// ");
            sb.Append($"{string.Join(" - ", state1.Pits)} <= {state2.Store}");
            return sb.ToString();
        }
    }
}

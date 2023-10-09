namespace MancalaGame
{
    public interface IMancala : IMancalaState
    {
        bool IsFinished { get; }

        MancalaPlayer Player { get; }

        void NewGame();

        void MakeMove(MancalaPlayer activePlayer, int pitIndex);

        IReadOnlyCollection<MancalaStateUpdate> LastMoveUpdates { get; }
    }
}

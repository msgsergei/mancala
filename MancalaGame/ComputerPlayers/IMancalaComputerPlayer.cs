namespace MancalaGame.ComputerPlayers
{
    public interface IMancalaComputerPlayer
    {
        int GetNextMovePitIndex(MancalaPlayer playsFor, IMancalaState gameState);
    }
}

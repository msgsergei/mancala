namespace MancalaGame
{
    public interface IMancalaState
    {
        MancalaPlayerState GetState(MancalaPlayer player);
    }
}

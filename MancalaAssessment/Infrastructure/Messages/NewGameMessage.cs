using MancalaGame.ComputerPlayers;

namespace MancalaWPF.Infrastructure.Messages
{
    public record NewGameMessage(string Player1Name, string Player2Name, IMancalaComputerPlayer? Player2Computer = null)
    {
        public static readonly NewGameMessage PlayerVsPlayer = new NewGameMessage(
            Player1Name: "Player #1",
            Player2Name: "Player #2");

        public static readonly NewGameMessage PlayerVsComputer = new NewGameMessage(
             Player1Name: "You",
             Player2Name: "T-1000",
             Player2Computer: new RandomMancalaComputerPlayer());
    }
}

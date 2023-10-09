namespace MancalaGame.ComputerPlayers
{
    public class RandomMancalaComputerPlayer : IMancalaComputerPlayer
    {
        private readonly Random _random;

        public RandomMancalaComputerPlayer()
            : this(new())
        {
        }

        internal RandomMancalaComputerPlayer(Random random)
        {
            _random = random;
        }

        public int GetNextMovePitIndex(MancalaPlayer playsFor, IMancalaState gameState)
        {
            var computerPlayerState = gameState.GetState(playsFor);

            var pitsWithNonZeroStones = computerPlayerState.Pits
                .Select((stones, index) => new { Stones = stones, Index = index })
                .Where(x => x.Stones > 0)
                .Select(x => x.Index)
                .ToArray();

            return pitsWithNonZeroStones[_random.Next(pitsWithNonZeroStones.Length)];
        }
    }
}

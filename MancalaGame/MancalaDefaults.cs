namespace MancalaGame
{
    internal static class MancalaDefaults
    {
        private const int NumberOfPitsPerPlayer = 6;
        private const int StonesPerPit = 4;

        public static readonly int DefaultStoreValue = 0;

        public static int[] CreateDefaultPits()
        {
            int[] pits = new int[NumberOfPitsPerPlayer];
            for (int i = 0; i < pits.Length; i++)
            {
                pits[i] = StonesPerPit;
            }
            return pits;
        }
    }
}

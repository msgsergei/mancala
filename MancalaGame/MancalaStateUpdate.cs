namespace MancalaGame
{
    public struct MancalaStateUpdate
    {
        public MancalaPlayer Player { get; internal set; }

        public int Change { get; internal set; }

        public int? PitIndex { get; internal set; }
    }
}

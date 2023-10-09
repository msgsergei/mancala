namespace MancalaGame
{
    public class MancalaPlayerState
    {
        private readonly int _initialStore;
        private readonly int[] _initialPits;

        private int _store;
        private int[] _pits = Array.Empty<int>();

        public int Store => _store;
        public IReadOnlyList<int> Pits => _pits;

        internal bool IsEmptyPits => _pits.All(stones => stones == 0);
        internal Action<MancalaPlayerStateUpdate>? StateUpdateHandler { get; set; }

        internal MancalaPlayerState(int store, int[] pits)
        {
            _initialStore = store;
            _initialPits = pits;
            Reset();
        }

        internal void Reset()
        {
            _store = _initialStore;
            _pits = _initialPits.ToArray();
        }

        internal int NumberOfStonesInPit(int pitIndex)
        {
            return _pits[pitIndex];
        }

        internal int TakeStonesFromPit(int pitIndex)
        {
            int stonesInPit = _pits[pitIndex];
            _pits[pitIndex] = 0;

            OnStateChanged(-stonesInPit, pitIndex);
            return stonesInPit;
        }

        internal void PutOneStoneIntoPit(int pitIndex)
        {
            _pits[pitIndex]++;
            OnStateChanged(1, pitIndex);
        }

        internal void PutStonesIntoStore(int numberOfStones)
        {
            _store += numberOfStones;
            OnStateChanged(numberOfStones, null);
        }

        internal void MoveAllStonesFromPitsIntoStore()
        {
            int stones = 0;
            for (int i = 0; i < _pits.Length; i++)
            {
                stones += TakeStonesFromPit(i);
            }
            PutStonesIntoStore(stones);
        }

        private void OnStateChanged(int change, int? pitIndex)
        {
            if (change != 0)
            {
                StateUpdateHandler?.Invoke(new() { Change = change, PitIndex = pitIndex });
            }
        }

        public override string ToString()
        {
            return $"{Store} => {string.Join(" - ", Pits)}";
        }
    }
}

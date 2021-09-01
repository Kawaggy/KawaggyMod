namespace KawaggyMod.Core.Structs
{
    public struct ItemDropInfo
    {
        public int type;
        public bool dropPerPlayer;
        public int min;
        public int max;

        public ItemDropInfo(int type, bool dropPerPlayer, int min, int max = -1)
        {
            this.type = type;
            this.dropPerPlayer = dropPerPlayer;
            this.min = min;
            this.max = max;
        }
    }
}

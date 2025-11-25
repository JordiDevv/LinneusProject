namespace Domain.Rarities
{
    public enum RarityType
    {
        Daily = 1,
        Peculiar = 2,
        Singular = 3,
        Majestic = 4,
        Epic = 5
    }
        
    public struct Rarity
    {
        public RarityType Type { get; private set; }

        public string Name => Type.ToString();
        public int Level => (int)Type;

        public Rarity(RarityType type)
        {
            Type = type;
        }
    }
}

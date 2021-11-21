using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootPiecesData
    {
        public string LootId;
        public Loot Loot;
        public Vector3Data Position;

        public LootPiecesData(string lootId, Loot loot, Vector3Data position)
        {
            LootId = lootId;
            Loot = loot;
            Position = position;
        }
    }
}
using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public List<LootOnLevel> LootOnLevel;
        public LootData LootData;

        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel);
            LootOnLevel = new List<LootOnLevel>();
            LootData = new LootData();
        }
    }
}
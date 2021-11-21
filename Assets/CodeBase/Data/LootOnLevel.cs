using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class LootOnLevel
    {
        public string Level;
        public List<LootPiecesData> LootsPiecesDatas;

        public LootOnLevel(string level)
        {
            Level = level;
            LootsPiecesDatas = new List<LootPiecesData>();
        }
    }
}
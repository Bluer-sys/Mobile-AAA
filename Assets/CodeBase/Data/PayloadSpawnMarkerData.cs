using System;
using CodeBase.StaticData;

namespace CodeBase.Data
{
    [Serializable]
    public class PayloadSpawnMarkerData
    {
        public string Id;
        public Vector3Data Position;
        public MonsterTypeId MonsterTypeId;

        public PayloadSpawnMarkerData(string id, Vector3Data position, MonsterTypeId monsterTypeId)
        {
            Id = id;
            Position = position;
            MonsterTypeId = monsterTypeId;
        }
    }
}
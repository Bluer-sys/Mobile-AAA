using System;
using CodeBase.StaticData;

namespace CodeBase.Data
{
    [Serializable]
    public class EnemySpawnerData
    {
        public string Id;
        public MonsterTypeId MonsterTypeId;
        public Vector3Data Position;

        public EnemySpawnerData(string id, MonsterTypeId monsterTypeId, Vector3Data position)
        {
            Id = id;
            MonsterTypeId = monsterTypeId;
            Position = position;
        }
    }
}
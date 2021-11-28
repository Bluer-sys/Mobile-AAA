using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;

        public List<EnemySpawnerData> EnemySpawners;
        public List<SaveTriggerData> SaveTriggers;

        public Vector3 HeroInitialPosition;
    }
}
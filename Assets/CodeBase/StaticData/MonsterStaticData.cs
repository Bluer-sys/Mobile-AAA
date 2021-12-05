using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;

        [Range(1, 100)]
        public int HP;

        [Range(1, 50)]
        public int Damage;

        [Range(1, 10)]
        public int MaxLoot;
        
        [Range(20, 50)]
        public int MinLoot;
        
        [Range(1, 5)]
        public float MoveSpeed;

        [Range(0.5f, 2.0f)]
        public float EffectiveDistance;
        
        [Range(0.5f, 1.0f)]
        public float Cleavage;

        public AssetReferenceGameObject PrefabReference;
    }
}
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(Vector3 initialPoint);
        GameObject CreateHud();
        GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
        LootPiece CreateLoot();
        void CreateSpawner(Vector3Data at, string spawnerId, MonsterTypeId monsterTypeId);
        void CreateSaveTrigger(string triggerId, Vector3Data at, Vector3Data size, Vector3Data center);
        GameObject HeroGameObject { get; }
        void Register(ISavedProgressReader progressReader);
    }
}
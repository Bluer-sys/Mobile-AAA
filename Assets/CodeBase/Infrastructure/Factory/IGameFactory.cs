using System;
using System.Collections.Generic;
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
        GameObject CreateHero(GameObject initialPoint);
        GameObject CreateHud();
        GameObject CreateMonster(MonsterTypeId typeId, Transform parent);
        LootPiece CreateLoot();
        void CreateSpawner(Vector3Data at, string spawnerId, MonsterTypeId monsterTypeId);
        GameObject HeroGameObject { get; }
        void Register(ISavedProgressReader progressReader);
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void CleanUp();
    }
}
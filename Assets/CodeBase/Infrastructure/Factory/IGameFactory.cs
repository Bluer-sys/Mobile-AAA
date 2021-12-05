using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Task<GameObject> CreateHero(Vector3 initialPoint);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent);
        Task<LootPiece> CreateLoot();
        Task CreateSpawner(Vector3Data at, string spawnerId, MonsterTypeId monsterTypeId);
        Task CreateSaveTrigger(string triggerId, Vector3Data at, Vector3Data size, Vector3Data center);

        Task CreateLevelTransferTrigger(string transferTriggerId, string transferTo, bool isActive, Vector3Data at,
            Vector3Data size, Vector3Data center);

        void CleanUp();
        Task WarmUp();
    }
}
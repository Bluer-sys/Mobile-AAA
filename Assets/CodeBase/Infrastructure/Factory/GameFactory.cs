using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.LevelTransfer;
using CodeBase.Logic.SaveTriggers;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _random;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressWatchersService _progressWatchersService;
        private readonly IWindowService _windowService;
        private readonly IGameStateMachine _stateMachine;

        public GameObject HeroGameObject { get; private set; }

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticData, IRandomService random,
            IPersistentProgressService progressService, IPersistentProgressWatchersService progressWatchersService,
            ISaveLoadService saveLoadService, IWindowService windowService, IGameStateMachine stateMachine)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _random = random;
            _progressService = progressService;
            _progressWatchersService = progressWatchersService;
            _saveLoadService = saveLoadService;
            _windowService = windowService;
            _stateMachine = stateMachine;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(AssetAddress.Loot);
            await _assetProvider.Load<GameObject>(AssetAddress.Spawner);
            await _assetProvider.Load<GameObject>(AssetAddress.SaveTrigger);
            await _assetProvider.Load<GameObject>(AssetAddress.LevelTransferTrigger);
        }

        public async Task<GameObject> CreateHero(Vector3 initialPoint)
        {
            HeroGameObject = await InstantiateRegisteredAsync(AssetAddress.Hero, initialPoint);

            return HeroGameObject;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.Hud);

            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindowButton.Construct(_windowService);
            }

            return hud;
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);

            GameObject prefab = await _assetProvider.Load<GameObject>(monsterData.PrefabReference);

            GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            IHealth health = monster.GetComponent<IHealth>();
            health.Current = monsterData.HP;
            health.Max = monsterData.HP;

            monster.GetComponent<ActorUI>().ConstructHealth(health);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;
            monster.GetComponent<Follow>().Construct(HeroGameObject.transform);

            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _random);
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);

            EnemyAttack attack = monster.GetComponent<EnemyAttack>();
            attack.Construct(HeroGameObject.transform);
            attack.Damage = monsterData.Damage;
            attack.Cleavage = monsterData.Cleavage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;

            return monster;
        }

        public async Task<LootPiece> CreateLoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.Loot);

            LootPiece lootPiece = InstantiateRegistered(prefab)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);

            return lootPiece;
        }

        public async Task CreateSpawner(Vector3Data at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            GameObject loadedPrefab = await _assetProvider.Load<GameObject>(AssetAddress.Spawner);

            SpawnPoint spawner = InstantiateRegistered(loadedPrefab, at.AsUnityVector())
                .GetComponent<SpawnPoint>();

            spawner.Construct(this);
            spawner.Id = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;
        }

        public async Task CreateSaveTrigger(string saveTriggerId, Vector3Data at, Vector3Data size, Vector3Data center)
        {
            GameObject loadedPrefab = await _assetProvider.Load<GameObject>(AssetAddress.SaveTrigger);

            SaveTrigger trigger = InstantiateRegistered(loadedPrefab, at.AsUnityVector())
                .GetComponent<SaveTrigger>();

            trigger.Construct(_saveLoadService);
            trigger.Id = saveTriggerId;

            BoxCollider collider = trigger.GetComponent<BoxCollider>();
            collider.size = size.AsUnityVector();
            collider.center = center.AsUnityVector();
        }

        public async Task CreateLevelTransferTrigger(string transferTriggerId, string transferTo, bool isActive, Vector3Data at, Vector3Data size, Vector3Data center)
        {
            GameObject loadedPrefab = await _assetProvider.Load<GameObject>(AssetAddress.LevelTransferTrigger);

            LevelTransferTrigger transferTrigger = InstantiateRegistered(loadedPrefab, at.AsUnityVector())
                .GetComponent<LevelTransferTrigger>();

            transferTrigger.Construct(_stateMachine);
            transferTrigger.Id = transferTriggerId;
            transferTrigger.TransferTo = transferTo;
            transferTrigger.IsActive = isActive;

            BoxCollider collider = transferTrigger.GetComponent<BoxCollider>();
            collider.size = size.AsUnityVector();
            collider.center = center.AsUnityVector();
            
            transferTrigger.gameObject.SetActive(false);
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assetProvider.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                _progressWatchersService.ProgressWriters.Add(progressWriter);
            }

            _progressWatchersService.ProgressReaders.Add(progressReader);
        }

        public void CleanUp()
        {
            _assetProvider.CleanUp();
        }
    }
}
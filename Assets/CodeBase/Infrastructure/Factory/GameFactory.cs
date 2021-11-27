using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
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
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _random;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressWatchersService _progressWatchersService;
        private readonly IWindowService _windowService;

        public GameObject HeroGameObject { get; private set; }

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService random,
            IPersistentProgressService progressService, IPersistentProgressWatchersService progressWatchersService,
            ISaveLoadService saveLoadService, IWindowService windowService)
        {
            _assets = assets;
            _staticData = staticData;
            _random = random;
            _progressService = progressService;
            _progressWatchersService = progressWatchersService;
            _saveLoadService = saveLoadService;
            _windowService = windowService;
        }

        public GameObject CreateHero(GameObject initialPoint)
        {
            HeroGameObject = InstantiateRegistered(AssetPath.Hero, initialPoint.transform.position);

            return HeroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.Hud);
            
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindowButton.Construct(_windowService);
            }
            
            return hud;
        }

        public GameObject CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);
            GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

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

        public LootPiece CreateLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetPath.Loot)
                .GetComponent<LootPiece>();
            
            lootPiece.Construct(_progressService.Progress.WorldData);
            
            return lootPiece;
        }

        public void CreateSpawner(Vector3Data at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            SpawnPoint spawner = InstantiateRegistered(AssetPath.Spawner, at.AsUnityVector())
                .GetComponent<SpawnPoint>();

            spawner.Construct(this);
            spawner.Id = spawnerId;
            spawner.MonsterTypeId = monsterTypeId;
        }

        public void CreateSaveTrigger(string saveTriggerId, Vector3Data at, Vector3Data size, Vector3Data center)
        {
            SaveTrigger trigger = InstantiateRegistered(AssetPath.SaveTrigger, at.AsUnityVector())
                .GetComponent<SaveTrigger>();

            trigger.Construct(_saveLoadService);
            trigger.Id = saveTriggerId;

            BoxCollider collider = trigger.GetComponent<BoxCollider>();
            collider.size = size.AsUnityVector();
            collider.center = center.AsUnityVector();
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
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
    }
}
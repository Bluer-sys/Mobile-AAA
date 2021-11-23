using System.Linq;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerInitialPointTag = "PlayerInitialPoint";
        private const string EnemySpawnerTag = "EnemySpawner";
        private const string SaveTriggerTag = "SaveTrigger";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.CleanUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private void InitGameWorld()
        {
            InitSpawners();
            InitSaveTriggers();
            InitLoot();

            GameObject hero = InitHero();
            InitHud(hero);

            CameraFollow(hero);
        }

        private void InitLoot()
        {
            LootOnLevel lootPiecesOnLevel =
                _progressService.Progress.WorldData.LootOnLevel.FirstOrDefault(loot =>
                    loot.Level == SceneManager.GetActiveScene().name);

            if (lootPiecesOnLevel != null)
            {
                foreach (var currentLootPiece in lootPiecesOnLevel.LootsPiecesDatas)
                {
                    LootPiece loadedLoot = _gameFactory.CreateLoot();

                    loadedLoot.Initialize(currentLootPiece.Loot);
                    loadedLoot.transform.position = currentLootPiece.Position.AsUnityVector();
                    loadedLoot.GetComponent<UniqueId>().Id = currentLootPiece.LootId;
                }
                lootPiecesOnLevel.LootsPiecesDatas.Clear();
            }
        }

        private void InitSpawners()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticData.ForLevel(sceneKey);
            
            foreach (var spawnerData in levelData.EnemySpawners)
            {
                _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private void InitSaveTriggers()
        {
            foreach (var saveTriggerObject in GameObject.FindGameObjectsWithTag(SaveTriggerTag))
            {
                SaveTrigger saveTrigger = saveTriggerObject.GetComponent<SaveTrigger>();
                _gameFactory.Register(saveTrigger);
            }
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            
            hud.GetComponentInChildren<ActorUI>()
                .ConstructHealth(hero.GetComponent<HeroHealth>());
        }

        private GameObject InitHero() => 
            _gameFactory.CreateHero(initialPoint: GameObject.FindWithTag(PlayerInitialPointTag));

        private void CameraFollow(GameObject hero)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);
        }
    }
}
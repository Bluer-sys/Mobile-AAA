using System.Linq;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressWatchersService _progressWatchersService;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IPersistentProgressService progressService, IStaticDataService staticData, IPersistentProgressWatchersService progressWatchersService, IGameFactory gameFactory, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _progressService = progressService;
            _staticData = staticData;
            _progressWatchersService = progressWatchersService;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _progressWatchersService.CleanUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _progressWatchersService.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private void InitUIRoot()
        {
            _uiFactory.CreateUIRoot();
        }

        private void InitGameWorld()
        {
            var levelData = LevelStaticData();

            InitSpawners(levelData);
            InitSaveTriggers(levelData);
            InitLoot();

            GameObject hero = InitHero(levelData);
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
                foreach (LootPiecesData currentLootPiece in lootPiecesOnLevel.LootsPiecesDatas)
                {
                    LootPiece loadedLoot = _gameFactory.CreateLoot();

                    loadedLoot.Initialize(currentLootPiece.Loot);
                    loadedLoot.transform.position = currentLootPiece.Position.AsUnityVector();
                    loadedLoot.GetComponent<UniqueId>().Id = currentLootPiece.LootId;
                }
                lootPiecesOnLevel.LootsPiecesDatas.Clear();
            }
        }

        private void InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private void InitSaveTriggers(LevelStaticData levelData)
        {
            foreach (SaveTriggerData triggerData in levelData.SaveTriggers)
            {
                _gameFactory.CreateSaveTrigger(triggerData.Id, triggerData.Position, triggerData.Size, triggerData.Center);
            }
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            
            hud.GetComponentInChildren<ActorUI>()
                .ConstructHealth(hero.GetComponent<HeroHealth>());
        }

        private GameObject InitHero(LevelStaticData levelData) =>
            _gameFactory.CreateHero(initialPoint: levelData.HeroInitialPosition);

        private LevelStaticData LevelStaticData() =>
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        private void CameraFollow(GameObject hero)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);
        }
    }
}
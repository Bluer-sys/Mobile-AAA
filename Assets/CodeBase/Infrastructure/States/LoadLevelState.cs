using System.Linq;
using System.Threading.Tasks;
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

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain,
            IPersistentProgressService progressService, IStaticDataService staticData,
            IPersistentProgressWatchersService progressWatchersService, IGameFactory gameFactory, IUIFactory uiFactory)
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

            _gameFactory.CleanUp();
            _gameFactory.WarmUp();

            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();
            InformProgressReaders();

            _gameStateMachine.Enter<LoadIAPState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _progressWatchersService.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private async Task InitUIRoot()
        {
            await _uiFactory.CreateUIRoot();
        }

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            await InitSpawners(levelData);
            await InitSaveTriggers(levelData);
            await InitLevelTransferTriggers(levelData);
            await InitHealthPotions(levelData);
            await InitLoot();

            GameObject hero = await InitHero(levelData);
            await InitHud(hero);

            CameraFollow(hero);
        }

        private async Task InitLoot()
        {
            LootOnLevel lootPiecesOnLevel =
                _progressService.Progress.WorldData.LootOnLevel.FirstOrDefault(loot =>
                    loot.Level == SceneManager.GetActiveScene().name);

            if (lootPiecesOnLevel != null)
            {
                foreach (LootPiecesData currentLootPiece in lootPiecesOnLevel.LootsPiecesDatas)
                {
                    LootPiece loadedLoot = await _gameFactory.CreateLoot();

                    loadedLoot.Initialize(currentLootPiece.Loot);
                    loadedLoot.transform.position = currentLootPiece.Position.AsUnityVector();
                    loadedLoot.GetComponent<UniqueId>().Id = currentLootPiece.LootId;
                }

                lootPiecesOnLevel.LootsPiecesDatas.Clear();
            }
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private async Task InitSaveTriggers(LevelStaticData levelData)
        {
            foreach (SaveTriggerData triggerData in levelData.SaveTriggers)
            {
                await _gameFactory.CreateSaveTrigger(triggerData.Id, triggerData.Position, triggerData.Size,
                    triggerData.Center);
            }
        }

        private async Task InitLevelTransferTriggers(LevelStaticData levelData)
        {
            foreach (LevelTransferData levelTransfer in levelData.LevelTransfers)
            {
                await _gameFactory.CreateLevelTransferTrigger(levelTransfer.Id, levelTransfer.TransferTo, 
                    levelTransfer.IsActive, levelTransfer.Position, levelTransfer.Size, levelTransfer.Center, levelTransfer.Rotation, 
                    levelTransfer.PayloadSpawnMarkerDatas);
            }
        }

        private async Task InitHealthPotions(LevelStaticData levelData)
        {
            foreach (HealthPotionData healthPotion in levelData.HealthPotions)
            {
                if (!_progressService.Progress.PickedPotionsData.PotionsId.Contains(healthPotion.Id))
                {
                    await _gameFactory.CreateHealthPotion(healthPotion.Id, healthPotion.Healing, healthPotion.Position);
                }
            }
        }

        private async Task InitHud(GameObject hero)
        {
            GameObject hud = await _gameFactory.CreateHud();

            hud.GetComponentInChildren<ActorUI>()
                .ConstructHealth(hero.GetComponent<HeroHealth>());
        }

        private async Task<GameObject> InitHero(LevelStaticData levelData) =>
            await _gameFactory.CreateHero(initialPoint: levelData.HeroInitialPosition);

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
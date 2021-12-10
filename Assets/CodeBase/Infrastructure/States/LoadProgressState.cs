using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string InitLevel = "Main";
        private const int InitHP = 50;
        private const int InitDamage = 5;
        private const float InitDamageRadius = 0.5f;

        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticData;


        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService, IStaticDataService staticData)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _staticData = staticData;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            PlayerProgress progress = new PlayerProgress(initialLevel: InitLevel);

            progress.HeroState.MaxHP = InitHP;
            progress.HeroStats.Damage = InitDamage;
            progress.HeroStats.DamageRadius = InitDamageRadius;

            CreateActiveLevelTransfers(progress);

            progress.HeroState.ResetHP();
            
            return progress;
        }

        private void CreateActiveLevelTransfers(PlayerProgress progress)
        {
            foreach (LevelTransferData transfer in _staticData.ForLevel(InitLevel).LevelTransfers)
            {
                if (transfer.IsActive)
                {
                    progress.ActiveLevelTransfersData.TriggersId.Add(transfer.Id);
                }
            }
        }
    }
}

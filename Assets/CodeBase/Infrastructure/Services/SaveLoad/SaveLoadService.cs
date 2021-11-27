using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IPersistentProgressService _playerProgress;
        private readonly IPersistentProgressWatchersService _progressWatchersService;

        public SaveLoadService(IPersistentProgressService playerProgress, IPersistentProgressWatchersService progressWatchersService)
        {
            _playerProgress = playerProgress;
            _progressWatchersService = progressWatchersService;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _progressWatchersService.ProgressWriters)
            {
                progressWriter.SaveProgress(_playerProgress.Progress);
            }

            PlayerPrefs.SetString(ProgressKey, _playerProgress.Progress.ToJson());
        }

        public PlayerProgress LoadProgress()
        {
           return PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();
        }
    }
}

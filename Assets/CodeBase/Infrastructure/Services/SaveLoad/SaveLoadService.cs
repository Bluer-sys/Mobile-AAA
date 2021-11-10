using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IPersistentProgressService _playerProgress;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService playerProgress, IGameFactory gameFactory)
        {
            _playerProgress = playerProgress;
            _gameFactory = gameFactory;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
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

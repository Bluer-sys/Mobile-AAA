using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsListener
    {
        private const string AndroidGameId = "4476131";
        private const string IOSGameId = "4476130";

        private const string RewardedVideoPlacementIdAndroid = "Rewarded_Android";
        private const string RewardedVideoPlacementIdIOS = "Rewarded_iOS";

        private string _gameId;
        private string _placementId;

        private Action _onVideoFinished;

        public event Action RewardedVideoReady;

        public int Reward => 50;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    _placementId = RewardedVideoPlacementIdAndroid;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    _placementId = RewardedVideoPlacementIdIOS;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = AndroidGameId;
                    _placementId = RewardedVideoPlacementIdAndroid;
                    break;
                default:
                    Debug.Log("This platform doesn't support for ads!");
                    break;
            }

            Advertisement.AddListener(this);
            Advertisement.Initialize(_gameId);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            _onVideoFinished = onVideoFinished;
            Advertisement.Show(_placementId);
        }

        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"OnUnityAdsReady {placementId}");

            if (placementId == _placementId)
            {
                RewardedVideoReady?.Invoke();
            }
        }

        public bool IsRewardedVideoReady() => Advertisement.IsReady(_placementId);

        public void OnUnityAdsDidError(string message) => Debug.LogError($"OnUnityAdsDidError {message}");

        public void OnUnityAdsDidStart(string placementId) => Debug.Log($"OnUnityAdsDidStart {placementId}");

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Skipped:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Finished:
                    _onVideoFinished?.Invoke();
                    break;
                default:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
            }

            _onVideoFinished = null;
        }
    }
}
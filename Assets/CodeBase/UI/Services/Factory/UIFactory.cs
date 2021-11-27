using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _persistentProgressService;

        private Transform _uiRoot;

        public UIFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService persistentProgressService)
        {
            _assets = assets;
            _staticData = staticData;
            _persistentProgressService = persistentProgressService;
        }

        public void CreateUIRoot()
        { 
            _uiRoot = _assets.Instantiate(AssetPath.UIRoot).transform;
        }

        public void CreateShop()
        {
            WindowConfig windowConfig = _staticData.ForWindow(WindowId.Shop);
            WindowBase shop = Object.Instantiate(windowConfig.Prefab, _uiRoot);
            shop.Construct(_persistentProgressService);
        }
    }
}
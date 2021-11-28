using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.UIWindows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IAdsService _adsService;

        private Transform _uiRoot;

        public UIFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService persistentProgressService, IAdsService adsService)
        {
            _assets = assets;
            _staticData = staticData;
            _persistentProgressService = persistentProgressService;
            _adsService = adsService;
        }

        public void CreateUIRoot()
        { 
            _uiRoot = _assets.Instantiate(AssetPath.UIRoot).transform;
        }

        public void CreateShop()
        {
            WindowConfig windowConfig = _staticData.ForWindow(WindowId.Shop);
            ShopWindow shop = Object.Instantiate(windowConfig.Prefab, _uiRoot) as ShopWindow;
            shop.Construct(_persistentProgressService, _adsService);
        }
    }
}
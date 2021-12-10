using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
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
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IAdsService _adsService;
        private readonly IIAPService _iapService;

        private Transform _uiRoot;

        public UIFactory(IAssetProvider assetProvider, IStaticDataService staticData, IPersistentProgressService persistentProgressService, IAdsService adsService, IIAPService iapService)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _persistentProgressService = persistentProgressService;
            _adsService = adsService;
            _iapService = iapService;
        }

        public async Task CreateUIRoot()
        {
            GameObject uiRootInstance = await _assetProvider.Instantiate(AssetAddress.UIRoot);
            _uiRoot = uiRootInstance.transform;
        }

        public void CreateShop()
        {
            WindowConfig windowConfig = _staticData.ForWindow(WindowId.Shop);
            ShopWindow shop = Object.Instantiate(windowConfig.Prefab, _uiRoot) as ShopWindow;
            shop.Construct(_persistentProgressService, _adsService, _iapService, _assetProvider);
        }
    }
}
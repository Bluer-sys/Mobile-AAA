using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.UIWindows.Shop
{
    public class ShopItemsContainer : MonoBehaviour
    {
        public GameObject[] ShopUnavailableObjects;
        public Transform Parent;
        
        private readonly List<GameObject> _shopItems = new List<GameObject>();

        private IIAPService _iapService;
        private IPersistentProgressService _progressService;
        private IAssetProvider _assetProvider;

        public void Construct(IIAPService iapService, IPersistentProgressService progressService,
            IAssetProvider assetProvider)
        {
            _iapService = iapService;
            _progressService = progressService;
            _assetProvider = assetProvider;
        }

        public void Initialize()
        {
            RefreshAvailableItems();
        }

        public void Subscribe()
        {
            _iapService.Initialized += RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
        }

        public void CleanUp()
        {
            _iapService.Initialized -= RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
        }

        private async void RefreshAvailableItems()
        {
            UpdateUnavailableObjects();

            if (!_iapService.IsInitialized)
                return;

            ClearShopItems();

            await FillShopItems();
        }

        private void ClearShopItems()
        {
            foreach (GameObject shopItem in _shopItems)
            {
                Destroy(shopItem);
            }
        }

        private async Task FillShopItems()
        {
            foreach (ProductDescription productDescription in _iapService.Products())
            {
                GameObject shopItemObject = await _assetProvider.Instantiate(AssetAddress.ShopItem, Parent);
                ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();

                shopItem.Construct(_iapService, _assetProvider, productDescription);
                shopItem.Initialize();

                _shopItems.Add(shopItemObject);
            }
        }

        private void UpdateUnavailableObjects()
        {
            foreach (GameObject unavailableObject in ShopUnavailableObjects)
            {
                unavailableObject.SetActive(!_iapService.IsInitialized);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPService : IIAPService
    {
        private readonly IAPProvider _iapProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly IAssetProvider _assetProvider;

        public bool IsInitialized => _iapProvider.IsInitialized;
        public event Action Initialized;

        public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService, IAssetProvider assetProvider)
        {
            _iapProvider = iapProvider;
            _progressService = progressService;
            _assetProvider = assetProvider;
        }

        public async Task Initialize()
        {
            await _iapProvider.Initialize(this, _assetProvider);
            _iapProvider.Initialized += () => Initialized?.Invoke();
        }

        public void StartPurchase(string purchaseId) =>
            _iapProvider.StartPurchase(purchaseId);

        public List<ProductDescription> Products()
        {
            return ProductsDescriptions().ToList();
        }

        public PurchaseProcessingResult ProcessPurchase(string id)
        {
            ProductConfig productConfig = _iapProvider.Configs[id];

            switch (productConfig.ItemType)
            {
                case ItemType.Skulls:
                    _progressService.Progress.WorldData.LootData.Collect(productConfig.Quantity);
                    _progressService.Progress.PurchaseData.AddPurchase(id);
                    break;
            }

            return PurchaseProcessingResult.Complete;
        }

        private IEnumerable<ProductDescription> ProductsDescriptions()
        {
            PurchaseData purchaseData = _progressService.Progress.PurchaseData;

            foreach (string productId in _iapProvider.Products.Keys)
            {
                ProductConfig productConfig = _iapProvider.Configs[productId];
                Product product = _iapProvider.Products[productId];

                BoughtIAP boughtIAP = purchaseData.BoughtIaps.Find(x => x.IAPid == productId);

                if (ProductBoughtOut(boughtIAP, productConfig))
                    continue;

                yield return new ProductDescription
                {
                    Id = productId,
                    ProductConfig = productConfig,
                    Product = product,
                    AvailablePurchasesLeft = boughtIAP != null 
                        ? productConfig.MaxPurchaseCount - boughtIAP.Count
                        : productConfig.MaxPurchaseCount,
                };
            }
        }

        private bool ProductBoughtOut(BoughtIAP boughtIAP, ProductConfig productConfig) =>
            boughtIAP != null && boughtIAP.Count >= productConfig.MaxPurchaseCount;
    }
}
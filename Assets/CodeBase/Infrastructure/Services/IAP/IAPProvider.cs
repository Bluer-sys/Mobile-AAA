using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class IAPProvider : IStoreListener
    {
        public Dictionary<string, ProductConfig> Configs { get; private set; }
        public Dictionary<string, Product> Products { get; private set; }
        
        private IIAPService _iapService;
        private IAssetProvider _assetProvider;

        private IStoreController _controller;
        private IExtensionProvider _extensions;

        public event Action Initialized;

        public bool IsInitialized => _controller != null && _extensions != null;

        public async Task Initialize(IIAPService iapService, IAssetProvider assetProvider)
        {
            _iapService = iapService;
            _assetProvider = assetProvider;
            
            Configs = new Dictionary<string, ProductConfig>();
            Products = new Dictionary<string, Product>();
            
            await Load();
            
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (ProductConfig productConfig in Configs.Values)
            {
                builder.AddProduct(productConfig.Id, productConfig.ProductType);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void StartPurchase(string purchaseId)
        {
            _controller.InitiatePurchase(purchaseId);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;

            foreach (Product product in _controller.products.all)
            {
                Products.Add(product.definition.id, product);
            }
            
            Initialized?.Invoke();

            Debug.Log("UnityPurchasing init success");
        }

        public void OnInitializeFailed(InitializationFailureReason error) =>
            Debug.LogError($"UnityPurchasing OnInitializeFailed {error}");

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"UnityPurchasing ProcessPurchase success, {purchaseEvent.purchasedProduct.definition.id}");

            return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct.definition.id);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
            Debug.LogError(
                $"Product {product.definition.id} purchase failed, PurchaseFailureReason {failureReason}, transaction ID: {product.transactionID}");
        
        private async Task Load()
        {
            TextAsset products = await _assetProvider.Load<TextAsset>(AssetAddress.IAPProducts);

            Configs = products
                .text
                .ToDeserialized<ProductConfigWrapper>()
                .Configs
                .ToDictionary(x => x.Id, x => x);
        }
    }
}
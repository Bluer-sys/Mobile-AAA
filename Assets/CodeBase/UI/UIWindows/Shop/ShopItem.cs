using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.UIWindows.Shop
{
    public class ShopItem : MonoBehaviour
    {
        public Button BuyItemButton;
        public TextMeshProUGUI PriceText;
        public TextMeshProUGUI QuantityText;
        public TextMeshProUGUI AvailableItemsLeftText;
        public Image Icon;

        private IIAPService _iapService;
        private IAssetProvider _assetProvider;
        
        private ProductDescription _productDescription;

        public void Construct(IIAPService iapService, IAssetProvider assetProvider, ProductDescription productDescription)
        {
            _iapService = iapService;
            _assetProvider = assetProvider;
            
            _productDescription = productDescription;
        }

        public async void Initialize()
        {
            BuyItemButton.onClick.AddListener(OnBuyItemClick);
            
            PriceText.text = _productDescription.ProductConfig.Price;
            QuantityText.text = _productDescription.ProductConfig.Quantity.ToString();
            AvailableItemsLeftText.text = _productDescription.AvailablePurchasesLeft.ToString();
            Icon.sprite = await _assetProvider.Load<Sprite>(_productDescription.ProductConfig.Icon);
        }

        private void OnDestroy()
        {
            BuyItemButton.onClick.RemoveListener(OnBuyItemClick);
        }

        private void OnBuyItemClick()
        {
            _iapService.ProcessPurchase(_productDescription.Id);
        }
    }
}
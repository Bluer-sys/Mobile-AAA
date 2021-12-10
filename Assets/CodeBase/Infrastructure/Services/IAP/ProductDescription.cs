using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public class ProductDescription
    {
        public string Id;
        public ProductConfig ProductConfig;
        public Product Product;
        public int AvailablePurchasesLeft;
    }
}
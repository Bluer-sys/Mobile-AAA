using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP
{
    public interface IIAPService : IService
    {
        bool IsInitialized { get; }
        event Action Initialized;
        Task Initialize();
        List<ProductDescription> Products();
        PurchaseProcessingResult ProcessPurchase(string id);
    }
}
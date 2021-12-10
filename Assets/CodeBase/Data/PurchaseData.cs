using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PurchaseData
    {
        public List<BoughtIAP> BoughtIaps = new List<BoughtIAP>();

        public event Action Changed;
        
        public void AddPurchase(string id)
        {
            BoughtIAP iap = BoughtIaps.Find(x => x.IAPid == id);

            if (iap != null)
            {
                iap.Count++;
            }
            else
            {
                BoughtIaps.Add(new BoughtIAP(id, 1));
            }
            
            Changed?.Invoke();
        }
    }
}
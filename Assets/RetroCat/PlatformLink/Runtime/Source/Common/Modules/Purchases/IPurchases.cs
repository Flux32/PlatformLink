using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    public interface IPurchases
    {
        public event Action PurchaseStarted;
        public event Action<Purchase> Purchased;
        public event Action<string> PurchaseFailed;
        
        public void Purchase(string id);
        
        public Purchase[] GetPurchases();
        public void ConsumePurchase(Purchase purchase);
        public void GetCatalog(Action<bool, CatalogProduct[]> onCompleted);
    }
}

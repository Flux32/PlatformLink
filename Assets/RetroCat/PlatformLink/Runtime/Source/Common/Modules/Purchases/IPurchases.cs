using System;

namespace PlatformLink.Common
{
    public interface IPurchases
    {
        public event Action PurchaseStarted;
        public event Action<Purchase> Purchased;
        public event Action<string> PurchaseFailed;
        
        public void Purchase(string id);
    }
}
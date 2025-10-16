using System;
using System.Runtime.InteropServices;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Purchases
{
    public class YandexPurchases : MonoBehaviour, IPurchases
    {
        public event Action PurchaseStarted;
        public event Action<Purchase> Purchased;
        public event Action<string> PurchaseFailed;

        [DllImport("__Internal")]
        private static extern void jslib_purchase(string id);
        private static extern void jslib_consumePurchase(string token);
        
        public void Purchase(string id)
        {
            jslib_purchase(id);
        }

        public Purchase[] GetPurchases()
        {
            throw new NotImplementedException();
        }

        public void ConsumePurchase(Purchase purchase)
        {
            throw new NotImplementedException();
        }

        #region Called from PlatformLink.js
        private void fjs_onPurchaseSuccess()
        {
            Purchased?.Invoke(new Purchase("", "0", ProductType.Consumable));
        }

        private void fjs_onPurchaseError()
        {
            PurchaseFailed?.Invoke("Purchase failed");
        }
        #endregion
    }
}
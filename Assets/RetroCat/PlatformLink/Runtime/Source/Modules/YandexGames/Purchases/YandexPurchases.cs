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
        
        private Action<bool, CatalogProduct[]> _getCatalogCompleted;
        private Action<bool, CatalogProduct> _getProductCompleted;

        [DllImport("__Internal")]
        private static extern void jslib_purchase(string id);
        private static extern void jslib_consumePurchase(string token);
        [DllImport("__Internal")]
        private static extern void jslib_getCatalog();
        [DllImport("__Internal")]
        private static extern void jslib_getProduct(string id);
        
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

        public void GetCatalog(Action<bool, CatalogProduct[]> onCompleted)
        {
            _getCatalogCompleted = onCompleted;
            jslib_getCatalog();
        }

        public void GetProduct(string id, Action<bool, CatalogProduct> onCompleted)
        {
            _getProductCompleted = onCompleted;
            jslib_getProduct(id);
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

        private void fjs_onGetCatalogSuccess(string json)
        {
            try
            {
                // Use Unity JsonUtility with a wrapper to parse arrays
                var wrapper = JsonUtility.FromJson<CatalogWrapper>(json);
                if (wrapper?.items == null)
                {
                    _getCatalogCompleted?.Invoke(false, Array.Empty<CatalogProduct>());
                    return;
                }

                CatalogProduct[] result = new CatalogProduct[wrapper.items.Length];
                for (int i = 0; i < wrapper.items.Length; i++)
                {
                    var p = wrapper.items[i];
                    result[i] = new CatalogProduct(
                        p.id ?? string.Empty,
                        p.title ?? string.Empty,
                        p.description ?? string.Empty,
                        new PriceCurrencyIcon(p.iconUrl ?? string.Empty),
                        p.price ?? string.Empty,
                        p.priceValue ?? string.Empty,
                        p.priceCurrencyCode ?? string.Empty);
                }

                _getCatalogCompleted?.Invoke(true, result);
            }
            catch (Exception)
            {
                _getCatalogCompleted?.Invoke(false, Array.Empty<CatalogProduct>());
            }
        }

        private void fjs_onGetCatalogFailed()
        {
            _getCatalogCompleted?.Invoke(false, Array.Empty<CatalogProduct>());
        }

        private void fjs_onGetProductSuccess(string json)
        {
            try
            {
                var p = JsonUtility.FromJson<CatalogProductJson>(json);
                if (p == null)
                {
                    _getProductCompleted?.Invoke(false, null);
                    return;
                }

                var product = new CatalogProduct(
                    p.id ?? string.Empty,
                    p.title ?? string.Empty,
                    p.description ?? string.Empty,
                    new PriceCurrencyIcon(p.iconUrl ?? string.Empty),
                    p.price ?? string.Empty,
                    p.priceValue ?? string.Empty,
                    p.priceCurrencyCode ?? string.Empty);

                _getProductCompleted?.Invoke(true, product);
            }
            catch (Exception)
            {
                _getProductCompleted?.Invoke(false, null);
            }
        }

        private void fjs_onGetProductFailed()
        {
            _getProductCompleted?.Invoke(false, null);
        }
        #endregion

        [Serializable]
        private class CatalogWrapper
        {
            public CatalogProductJson[] items;
        }

        [Serializable]
        private class CatalogProductJson
        {
            public string id;
            public string title;
            public string description;
            public string iconUrl;
            public string price;
            public string priceValue;
            public string priceCurrencyCode;
        }
    }
}

using System;
using System.Collections.Generic;
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
        private Action<Purchase[]> _getPurchasesCompleted;
        private readonly HashSet<string> _pendingConsumptionTokens = new HashSet<string>();

        [DllImport("__Internal")]
        private static extern void jslib_purchase(string id);
        [DllImport("__Internal")]
        private static extern void jslib_consumePurchase(string token);
        [DllImport("__Internal")]
        private static extern void jslib_getPurchases();
        [DllImport("__Internal")]
        private static extern void jslib_getCatalog();
        [DllImport("__Internal")]
        private static extern void jslib_getProduct(string id);
        
        public void Purchase(string id)
        {
            jslib_purchase(id);
        }

        public void GetPurchases(Action<Purchase[]> onCompleted)
        {
            _getPurchasesCompleted = onCompleted;
            jslib_getPurchases();
        }

        public void ConsumePurchase(Purchase purchase)
        {
            if (purchase == null)
            {
                Debug.LogWarning("YandexPurchases: ConsumePurchase called with null purchase");
                return;
            }

            if (string.IsNullOrEmpty(purchase.Token))
            {
                Debug.LogWarning($"YandexPurchases: Purchase {purchase.ProductId} has no token to consume");
                return;
            }

            if (_pendingConsumptionTokens.Contains(purchase.Token))
            {
                return;
            }

            _pendingConsumptionTokens.Add(purchase.Token);
            jslib_consumePurchase(purchase.Token);
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
                        new RemoteIcon(p.iconUrl ?? string.Empty),
                        new RemoteIcon(p.currencyIconUrl ?? string.Empty),
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
                    new RemoteIcon(p.iconUrl ?? string.Empty),
                    new RemoteIcon(p.currencyIconUrl ?? string.Empty),
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

        private void fjs_onGetPurchasesSuccess(string json)
        {
            try
            {
                var wrapper = JsonUtility.FromJson<PurchasesWrapper>(json);
                Purchase[] result = Array.Empty<Purchase>();
                if (wrapper?.items != null)
                {
                    result = new Purchase[wrapper.items.Length];
                    for (int i = 0; i < wrapper.items.Length; i++)
                    {
                        var purchase = wrapper.items[i];
                        result[i] = new Purchase(
                            purchase.productID ?? string.Empty,
                            purchase.purchaseToken ?? string.Empty,
                            ProductType.Consumable);
                    }
                }

                _getPurchasesCompleted?.Invoke(result);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"YandexPurchases: Failed to parse purchases payload. {exception.Message}");
            }
        }

        private void fjs_onGetPurchasesFailed()
        {
            _getPurchasesCompleted?.Invoke(Array.Empty<Purchase>());
        }

        private void fjs_onConsumePurchaseSuccess(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _pendingConsumptionTokens.Remove(token);

                // No caching to update here
            }
            else
            {
                _pendingConsumptionTokens.Clear();
            }
        }

        private void fjs_onConsumePurchaseFailed(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _pendingConsumptionTokens.Remove(token);
            }

            Debug.LogWarning($"YandexPurchases: Failed to consume purchase with token {token}");
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
            public string currencyIconUrl;
            public string price;
            public string priceValue;
            public string priceCurrencyCode;
        }

        [Serializable]
        private class PurchasesWrapper
        {
            public PurchaseJson[] items;
        }

        [Serializable]
        private class PurchaseJson
        {
            public string productID;
            public string purchaseToken;
            public string developerPayload;
        }
    }
}

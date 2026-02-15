using System;
using System.Linq;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using UnityEngine;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Purchases
{
    public class EditorPurchases : IPurchases
    {
        public event Action PurchaseStarted;
        public event Action<Purchase> Purchased;
        public event Action<string> PurchaseFailed;

        private readonly ILogger _logger;
        private PurchasesEditorSettings _editorSettings;
        
        public EditorPurchases(ILogger logger, PurchasesEditorSettings editorSettings)
        {
            _logger = logger;
            _editorSettings = editorSettings;
        }
    
        public void Purchase(string id)
        {
            _logger.Log("Purchase started");
            PurchaseStarted?.Invoke();
        
            _logger.Log("Purchased");
            ProductSettings productSettings = _editorSettings.Products.FirstOrDefault(product => product.Id == id);

            if (productSettings == null)
            {
                _logger.LogError($"Product with id {id} not registered");
                PurchaseFailed?.Invoke(id);
                return;
            }
            
            Purchased?.Invoke(new Purchase(productSettings.Id,  "0", productSettings.ProductType));
        }

        public void GetPurchases(Action<Purchase[]> onCompleted)
        {
            onCompleted?.Invoke(Array.Empty<Purchase>());
        }

        public void ConsumePurchase(Purchase purchase)
        {
            Debug.Log("Purchase consumed");
        }

        public void GetCatalog(Action<bool, CatalogProduct[]> onCompleted)
        {
            ProductSettings[] products = PlatformLinkSettings.Instance.Editor.Purchases.Products;
            
            if (products == null || products.Length == 0)
            {
                onCompleted?.Invoke(true, Array.Empty<CatalogProduct>());
                return;
            }

            CatalogProduct[] catalog = new CatalogProduct[products.Length];
            for (int i = 0; i < products.Length; i++)
            {
                catalog[i] = new CatalogProduct(
                    products[i].Id,
                    string.Empty,
                    string.Empty,
                    new RemoteIcon(string.Empty),
                    new RemoteIcon(string.Empty),
                    string.Empty,
                    string.Empty,
                    string.Empty);
            }

            onCompleted?.Invoke(true, catalog);
        }

        public void GetProduct(string id, Action<bool, CatalogProduct> onCompleted)
        {
            ProductSettings[] products = PlatformLinkSettings.Instance.Editor.Purchases.Products;
            ProductSettings product = products?.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                onCompleted?.Invoke(false, null);
                return;
            }

            var result = new CatalogProduct(
                product.Id,
                string.Empty,
                string.Empty,
                new RemoteIcon(string.Empty),
                new RemoteIcon(string.Empty),
                string.Empty,
                string.Empty,
                string.Empty);
            onCompleted?.Invoke(true, result);
        }
    }
}

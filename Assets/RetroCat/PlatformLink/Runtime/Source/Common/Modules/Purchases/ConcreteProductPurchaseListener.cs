using PlatformLink;
using UnityEngine;
using UnityEngine.Events;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    public class ConcreteProductPurchaseListener : MonoBehaviour
    {
        [SerializeField] private string _productId;

        public UnityEvent<Purchase> Purchased;

        private void OnEnable()
        {
            PLink.Purchases.Purchased += OnPurchased;
        }

        private void OnDisable()
        {
            PLink.Purchases.Purchased -= OnPurchased;
        }

        private void OnPurchased(Purchase purchase)
        {
            if (purchase.ProductId == _productId)
                Purchased?.Invoke(purchase);
        }
    }
}
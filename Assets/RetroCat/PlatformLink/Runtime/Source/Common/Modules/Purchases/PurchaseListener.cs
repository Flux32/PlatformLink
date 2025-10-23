using PlatformLink;
using UnityEngine;
using UnityEngine.Events;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    public class AnyPurchaseListener : MonoBehaviour
    {
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
            Purchased?.Invoke(purchase);
        }
    }
}
using PlatformLink;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    public class PurchaseInvoker : MonoBehaviour
    {
        public void InitiatePurchase(string id)
        {
            PLink.Purchases.Purchase(id);
        }
    }
}
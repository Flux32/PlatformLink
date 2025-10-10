using PlatformLink;
using UnityEngine;

public class PurchaseInvoker : MonoBehaviour
{
    public void InitiatePurchase(string id)
    {
        PLink.Purchases.Purchase(id);
    }
}
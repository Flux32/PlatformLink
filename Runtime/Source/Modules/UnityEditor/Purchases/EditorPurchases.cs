using System;

public class EditorPurchases : IPurchases
{
    public event Action<Purchase> Purchased;
    public event Action<string> PurchaseFailed;
    
    public void Purchase(string id)
    {
        Purchased?.Invoke(new Purchase(id));
    }
}

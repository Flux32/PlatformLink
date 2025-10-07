using System;

public interface IPurchases
{
    public event Action<Purchase> Purchased;
    public event Action<string> PurchaseFailed;
    
    public void Purchase(string id);
}
using System;
using PlatformLink.Common;
using PlatformLink.PluginDebug;

public class EditorPurchases : IPurchases
{
    public event Action PurchaseStarted;
    public event Action<Purchase> Purchased;
    public event Action<string> PurchaseFailed;

    private readonly ILogger _logger;
    
    public EditorPurchases(ILogger logger)
    {
        _logger = logger;
    }
    
    public void Purchase(string id)
    {
        _logger.Log("Purchase started");
        PurchaseStarted?.Invoke();
        
        _logger.Log("Purchased");
        Purchased?.Invoke(new Purchase(id));
    }
}

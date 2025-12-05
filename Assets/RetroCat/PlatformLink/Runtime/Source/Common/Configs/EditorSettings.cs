using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using UnityEngine;

[Serializable]
public class EditorSettings
{
    [SerializeField] private EnvironmentEditorSettings _environment;
    [SerializeField] private StorageEditorSettings _storage;
    [SerializeField] private PurchasesEditorSettings _purchases;
    [SerializeField] private LeaderboardEditorSettings _leaderboard = new LeaderboardEditorSettings();
    
    public EnvironmentEditorSettings Environment => _environment;
    public StorageEditorSettings Storage => _storage;
    public PurchasesEditorSettings Purchases => _purchases;
    public LeaderboardEditorSettings Leaderboard => _leaderboard;
}

[Serializable]
public class PurchasesEditorSettings
{
    [SerializeField] private ProductSettings[] _products;
    
    public ProductSettings[] Products => _products;
}

[Serializable]
public class ProductSettings
{
    [SerializeField] private string _id;
    [SerializeField] private ProductType _productType;
    
    public string Id => _id;
    public ProductType ProductType => _productType;
}

[Serializable]
public class LeaderboardEditorSettings
{
    [SerializeField] private LeaderboardMockEntry[] _otherPlayers;

    public LeaderboardMockEntry[] OtherPlayers => _otherPlayers;

    public LeaderboardEditorSettings()
    {
        _otherPlayers = new[]
        {
            new LeaderboardMockEntry("Alice", 1500, "En", "mock_alice", string.Empty),
            new LeaderboardMockEntry("Bob", 1200, "En", "mock_bob", string.Empty),
            new LeaderboardMockEntry("Charlie", 900, "En", "mock_charlie", string.Empty),
        };
    }
}

[Serializable]
public class LeaderboardMockEntry
{
    [SerializeField] private string _publicName;
    [SerializeField] private int _score;
    [SerializeField] private string _lang;
    [SerializeField] private string _uniqueId;
    [SerializeField] private string _extraData;

    public string PublicName => _publicName;
    public int Score => _score;
    public string Lang => _lang;
    public string UniqueId => _uniqueId;
    public string ExtraData => _extraData;

    public LeaderboardMockEntry()
    {
    }

    public LeaderboardMockEntry(
        string publicName,
        int score,
        string lang,
        string uniqueId,
        string extraData)
    {
        _publicName = publicName;
        _score = score;
        _lang = lang;
        _uniqueId = uniqueId;
        _extraData = extraData;
    }
}

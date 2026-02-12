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
    [SerializeField] private PlatformGamesEditorSettings _platformGames = new PlatformGamesEditorSettings();
    
    public EnvironmentEditorSettings Environment => _environment;
    public StorageEditorSettings Storage => _storage;
    public PurchasesEditorSettings Purchases => _purchases;
    public LeaderboardEditorSettings Leaderboard => _leaderboard;
    public PlatformGamesEditorSettings PlatformGames => _platformGames;
}

[Serializable]
public class PlatformGamesEditorSettings
{
    [SerializeField] private string _developerUrl = string.Empty;
    [SerializeField] private EditorGameSettings[] _games = CreateDefaultGames();

    public string DeveloperUrl => _developerUrl;
    public EditorGameSettings[] Games => _games;

    public static EditorGameSettings[] CreateDefaultGames()
    {
        return new[]
        {
            new EditorGameSettings("puzzle", "Puzzle", string.Empty, string.Empty, string.Empty),
            new EditorGameSettings("match3", "Match 3", string.Empty, string.Empty, string.Empty),
            new EditorGameSettings("race", "Race", string.Empty, string.Empty, string.Empty),
            new EditorGameSettings("zombie", "Zombie", string.Empty, string.Empty, string.Empty),
            new EditorGameSettings("card", "Card", string.Empty, string.Empty, string.Empty),
        };
    }
}

[Serializable]
public class EditorGameSettings
{
    [SerializeField] private string _appId = string.Empty;
    [SerializeField] private string _title = string.Empty;
    [SerializeField] private string _url = string.Empty;
    [SerializeField] private string _coverUrl = string.Empty;
    [SerializeField] private string _iconUrl = string.Empty;

    public string AppId => _appId;
    public string Title => _title;
    public string Url => _url;
    public string CoverUrl => _coverUrl;
    public string IconUrl => _iconUrl;

    public EditorGameSettings()
    {
    }

    public EditorGameSettings(string appId, string title, string url, string coverUrl, string iconUrl)
    {
        _appId = appId;
        _title = title;
        _url = url;
        _coverUrl = coverUrl;
        _iconUrl = iconUrl;
    }
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

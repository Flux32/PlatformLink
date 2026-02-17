using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformLinkSettingsWindow : EditorWindow
{
    private const string HeaderText = "PlatformLink";
    private const string VisualTreePath = "Assets/RetroCat/PlatformLink/Editor/Resources/UI/PlatformLink.uxml";
    private const string PackageVisualTreePath = "Packages/com.retrocat.platformlink/Editor/Resources/UI/PlatformLink.uxml";
    
    private ListView _platformsList;
    private ScrollView _settingsScroll;

    private VisualElement _selectedTab;
    private int _selectedTabIndex;

    [MenuItem("Window/PlatformLink/Settings", false, int.MaxValue)]
    private static void Open()
    {
        PlatformLinkSettingsWindow window = (PlatformLinkSettingsWindow)GetWindow(typeof(PlatformLinkSettingsWindow));
        window.titleContent.text = HeaderText;
    }

    public class Tab
    {
        public Tab(string name, VisualElement tab)
        {
            Name = name;
            VisualElement = tab;
        }

        public string Name { get; private set; }
        public VisualElement VisualElement { get; private set; }
    }

    private void CreateGUI()
    {
        // Ensure a project-local settings asset exists to persist user changes
        PlatformLinkSettingsUtility.EnsureProjectSettingsAssetExists();

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(VisualTreePath);
        if (visualTree == null)
        {
            visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PackageVisualTreePath);
        }
        if (visualTree == null)
        {
            Debug.LogError($"PlatformLinkSettingsWindow: UXML not found at '{VisualTreePath}' or '{PackageVisualTreePath}'.");
            return;
        }

        visualTree.CloneTree(rootVisualElement);
        SerializedObject so = new SerializedObject(PlatformLinkSettings.Instance);
        rootVisualElement.Bind(so);

        _platformsList = rootVisualElement.Q<ListView>("platforms-list");
        _settingsScroll = rootVisualElement.Q<ScrollView>("settings-scroll");

        PropertyField editorTab = _settingsScroll.Q<PropertyField>("properly-editor");
        PropertyField androidTab = _settingsScroll.Q<PropertyField>("properly-android");
        PropertyField yandexGamesTab = _settingsScroll.Q<PropertyField>("properly-yandex-games");

        editorTab.BindProperty(so.FindProperty("_editor"));
        editorTab.Add(new HelpBox("Editor Platform Games: this list controls which games are returned by PLink.Platform.GetAllGames() while running in the Unity Editor.", HelpBoxMessageType.Info));
        editorTab.Add(new HelpBox("Editor Leaderboards: use 'Fake Loading Time (Seconds)' to simulate request latency for EditorLeaderboard.", HelpBoxMessageType.Info));
        editorTab.Add(new HelpBox("Editor Platform: toggle 'Authorized' to control the initial PLink.Platform.Authorized value in the Unity Editor.", HelpBoxMessageType.Info));
        
        androidTab.BindProperty(so.FindProperty("_android").FindPropertyRelative("_admobSettings"));
        androidTab.Add(new HelpBox("Google Mobile Ads App ID will look similar to this sample ID: ca-app-pub-3940256099942544~3347511713", HelpBoxMessageType.Info));

        _selectedTab = editorTab;

        androidTab.style.display = DisplayStyle.None;
        yandexGamesTab.style.display = DisplayStyle.None;
        yandexGamesTab.Add(new HelpBox("Soon))))", HelpBoxMessageType.Info));

        List<Tab> tabs = new List<Tab>()
        {
            new Tab("Editor", editorTab),
            new Tab("Android", androidTab),
            new Tab("Yandex Games", yandexGamesTab),
        };

        _platformsList.makeItem = () => new Label();
        _platformsList.bindItem = (item, index) => { (item as Label).text = tabs[index].Name; };
        _platformsList.itemsSource = tabs;

        _platformsList.selectionChanged += OnSelectionChanged;
        _platformsList.selectedIndex = _selectedTabIndex;
    }

    private void OnSelectionChanged(IEnumerable<object> selectedItems)
    {
        _selectedTab.style.display = DisplayStyle.None;
        Tab tab = selectedItems.First() as Tab;
        _selectedTab = tab.VisualElement;
        _selectedTab.style.display = DisplayStyle.Flex;
        _selectedTabIndex = _platformsList.selectedIndex;
    }
}

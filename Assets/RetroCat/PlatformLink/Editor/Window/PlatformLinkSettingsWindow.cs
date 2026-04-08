using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformLinkSettingsWindow : EditorWindow
{
    private const string HeaderText = "PlatformLink";
    private const string VisualTreePath = "Assets/RetroCat/PlatformLink/Editor/Resources/UI/PlatformLink.uxml";
    private const string PackageVisualTreePath = "Packages/com.retrocat.platformlink/Editor/Resources/UI/PlatformLink.uxml";

    private readonly List<PlatformSettingsEntry> _platforms = new List<PlatformSettingsEntry>();

    private PlatformLinkSettings _settings;
    private SerializedObject _serializedObject;
    private SerializedProperty _platformsProperty;
    private ListView _platformsList;
    private ToolbarMenu _addPlatformMenu;
    private VisualElement _settingsContent;

    [MenuItem("Window/PlatformLink/Settings", false, int.MaxValue)]
    private static void Open()
    {
        PlatformLinkSettingsWindow window = GetWindow<PlatformLinkSettingsWindow>();
        window.titleContent = new GUIContent(HeaderText);
    }

    private void CreateGUI()
    {
        PlatformLinkSettingsUtility.EnsureProjectSettingsAssetExists();

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(VisualTreePath);
        if (visualTree == null)
            visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PackageVisualTreePath);

        if (visualTree == null)
        {
            Debug.LogError($"PlatformLinkSettingsWindow: UXML not found at '{VisualTreePath}' or '{PackageVisualTreePath}'.");
            return;
        }

        rootVisualElement.Clear();
        visualTree.CloneTree(rootVisualElement);

        _settings = PlatformLinkSettings.Instance;
        _serializedObject = new SerializedObject(_settings);
        _platformsProperty = _serializedObject.FindProperty("_platforms");

        _platformsList = rootVisualElement.Q<ListView>("platforms-list");
        _addPlatformMenu = rootVisualElement.Q<ToolbarMenu>("add-platform-menu");
        _settingsContent = rootVisualElement.Q<VisualElement>("settings-content");

        ConfigureAddPlatformMenu();
        ConfigurePlatformsList();
        RefreshPlatforms();
        EnsureValidSelection();
        RenderSelectedPlatformSettings();
    }

    private void ConfigureAddPlatformMenu()
    {
        _addPlatformMenu.text = "+";
        _addPlatformMenu.tooltip = "Add platform";
        _addPlatformMenu.menu.AppendAction("Android", _ => AddPlatform(PlatformSettingsType.Android));
        _addPlatformMenu.menu.AppendAction("iOS", _ => AddPlatform(PlatformSettingsType.Ios));
        _addPlatformMenu.menu.AppendAction("Yandex Games", _ => AddPlatform(PlatformSettingsType.YandexGames));
    }

    private void ConfigurePlatformsList()
    {
        _platformsList.selectionType = SelectionType.Single;
        _platformsList.itemsSource = _platforms;
        _platformsList.makeItem = CreatePlatformRow;
        _platformsList.bindItem = BindPlatformRow;
        _platformsList.selectionChanged += _ => RenderSelectedPlatformSettings();
    }

    private VisualElement CreatePlatformRow()
    {
        PlatformListRowElement row = new PlatformListRowElement();
        row.RegisterCallback<MouseDownEvent>(eventData => HandleRowMouseDown(row, eventData), TrickleDown.TrickleDown);
        row.NameField.RegisterValueChangedCallback(_ => CommitRename(row));
        row.NameField.RegisterCallback<BlurEvent>(_ => EndRename(row));
        row.NameField.RegisterCallback<KeyDownEvent>(eventData => HandleRenameKeyDown(row, eventData));
        row.AddManipulator(new ContextualMenuManipulator(menuEvent => PopulateContextMenu(menuEvent.menu, row)));
        return row;
    }

    private void BindPlatformRow(VisualElement element, int index)
    {
        if (index < 0 || index >= _platforms.Count)
            return;

        PlatformListRowElement row = (PlatformListRowElement)element;
        PlatformSettingsEntry entry = _platforms[index];

        row.userData = index;
        row.TitleLabel.text = BuildPlatformTitle(entry);
        row.NameField.SetValueWithoutNotify(entry.Name);
        row.SetRenameMode(false);
    }

    private void PopulateContextMenu(DropdownMenu menu, PlatformListRowElement row)
    {
        if ((row.userData is int) == false)
            return;

        int index = (int)row.userData;

        menu.AppendAction("Rename", _ => BeginRename(row, index));
        menu.AppendAction(
            "Duplicate",
            _ => DuplicatePlatform(index),
            _ => _settings.CanDuplicatePlatform(index)
                ? DropdownMenuAction.Status.Normal
                : DropdownMenuAction.Status.Disabled);
        menu.AppendAction(
            "Delete",
            _ => RemovePlatform(index),
            _ => _settings.CanRemovePlatform(index)
                ? DropdownMenuAction.Status.Normal
                : DropdownMenuAction.Status.Disabled);
    }

    private static string BuildPlatformTitle(PlatformSettingsEntry entry)
    {
        string typeTag = PlatformLinkSettings.GetPlatformTypeTag(entry.Type);
        return $"{entry.Name} ({typeTag})";
    }

    private void HandleRowMouseDown(PlatformListRowElement row, MouseDownEvent eventData)
    {
        if ((row.userData is int) == false)
            return;

        int index = (int)row.userData;
        _platformsList.selectedIndex = index;

        bool isLeftDoubleClick = eventData.button == (int)MouseButton.LeftMouse && eventData.clickCount == 2;
        if (isLeftDoubleClick)
            BeginRename(row, index);
    }

    private void HandleRenameKeyDown(PlatformListRowElement row, KeyDownEvent eventData)
    {
        if (eventData.keyCode == KeyCode.Escape)
        {
            EndRename(row);
            eventData.StopPropagation();
        }
    }

    private void BeginRename(PlatformListRowElement row, int index)
    {
        _platformsList.selectedIndex = index;
        row.NameField.SetValueWithoutNotify(_platforms[index].Name);
        row.SetRenameMode(true);
        row.NameField.Focus();
        row.NameField.SelectAll();
    }

    private void EndRename(PlatformListRowElement row)
    {
        row.SetRenameMode(false);
    }

    private void CommitRename(PlatformListRowElement row)
    {
        if ((row.userData is int) == false)
        {
            EndRename(row);
            return;
        }

        int index = (int)row.userData;
        string platformName = row.NameField.value;

        Undo.RecordObject(_settings, "Rename Platform");
        _settings.RenamePlatform(index, platformName);
        ApplySettingsChanges();
        RefreshPlatforms();
        SetSelectedIndex(index);
    }

    private void AddPlatform(PlatformSettingsType type)
    {
        Undo.RecordObject(_settings, "Add Platform");
        int index = _settings.AddPlatform(type);
        ApplySettingsChanges();
        RefreshPlatforms();
        SetSelectedIndex(index);
    }

    private void DuplicatePlatform(int index)
    {
        Undo.RecordObject(_settings, "Duplicate Platform");
        int duplicatedIndex = _settings.DuplicatePlatform(index);
        ApplySettingsChanges();
        RefreshPlatforms();
        SetSelectedIndex(duplicatedIndex);
    }

    private void RemovePlatform(int index)
    {
        Undo.RecordObject(_settings, "Delete Platform");
        bool removed = _settings.RemovePlatform(index);
        if (removed == false)
            return;

        ApplySettingsChanges();
        RefreshPlatforms();
        SetSelectedIndex(Mathf.Clamp(index - 1, 0, _platforms.Count - 1));
    }

    private void RefreshPlatforms()
    {
        _platforms.Clear();
        _platforms.AddRange(_settings.Platforms);
        _platformsList.Rebuild();
        _serializedObject.Update();
        _platformsProperty = _serializedObject.FindProperty("_platforms");
    }

    private void EnsureValidSelection()
    {
        if (_platforms.Count == 0)
        {
            _platformsList.selectedIndex = -1;
            return;
        }

        if (_platformsList.selectedIndex < 0 || _platformsList.selectedIndex >= _platforms.Count)
            _platformsList.selectedIndex = 0;
    }

    private void SetSelectedIndex(int index)
    {
        if (_platforms.Count == 0)
        {
            _platformsList.selectedIndex = -1;
            RenderSelectedPlatformSettings();
            return;
        }

        int clampedIndex = Mathf.Clamp(index, 0, _platforms.Count - 1);
        _platformsList.selectedIndex = clampedIndex;
        RenderSelectedPlatformSettings();
    }

    private void RenderSelectedPlatformSettings()
    {
        _settingsContent.Clear();

        int index = _platformsList.selectedIndex;
        if (index < 0 || index >= _platforms.Count)
            return;

        _serializedObject.Update();
        _platformsProperty = _serializedObject.FindProperty("_platforms");

        SerializedProperty platformProperty = _platformsProperty.GetArrayElementAtIndex(index);
        SerializedProperty typeProperty = platformProperty.FindPropertyRelative("_type");
        PlatformSettingsType platformType = (PlatformSettingsType)typeProperty.enumValueIndex;
        SerializedProperty settingsProperty = GetSettingsProperty(platformProperty, platformType);

        if (settingsProperty == null)
            return;

        PropertyField settingsField = new PropertyField(settingsProperty);
        settingsField.BindProperty(settingsProperty);
        _settingsContent.Add(settingsField);
        AddHelpBoxes(platformType);
    }

    private void AddHelpBoxes(PlatformSettingsType type)
    {
        switch (type)
        {
            case PlatformSettingsType.Editor:
                _settingsContent.Add(new HelpBox("Editor Platform Games: this list controls which games are returned by PLink.Platform.GetAllGames() while running in the Unity Editor.", HelpBoxMessageType.Info));
                _settingsContent.Add(new HelpBox("Editor Leaderboards: use 'Fake Loading Time (Seconds)' to simulate request latency for EditorLeaderboard.", HelpBoxMessageType.Info));
                _settingsContent.Add(new HelpBox("Editor Platform: toggle 'Authorized' to control the initial PLink.Platform.Authorized value in the Unity Editor.", HelpBoxMessageType.Info));
                break;
            case PlatformSettingsType.Android:
            case PlatformSettingsType.Ios:
                _settingsContent.Add(new HelpBox("Google Mobile Ads App ID will look similar to this sample ID: ca-app-pub-3940256099942544~3347511713", HelpBoxMessageType.Info));
                break;
            case PlatformSettingsType.YandexGames:
                _settingsContent.Add(new HelpBox("Enable Yandex Metrika to inject counter code into every HTML file of the WebGL build.", HelpBoxMessageType.Info));
                _settingsContent.Add(new HelpBox("Counter ID is the numeric value from Metrika. It is used in all places of the counter snippet (tag.js?id=..., ym(...), /watch/...).", HelpBoxMessageType.Info));
                _settingsContent.Add(new HelpBox("Loading Screen > _manualClose keeps the loading overlay visible after Unity starts. Close it manually by calling PLink.Environment.CloseLoadingScreen().", HelpBoxMessageType.Info));
                break;
        }
    }

    private static SerializedProperty GetSettingsProperty(SerializedProperty platformProperty, PlatformSettingsType type)
    {
        switch (type)
        {
            case PlatformSettingsType.Editor:
                return platformProperty.FindPropertyRelative("_editorSettings");
            case PlatformSettingsType.Android:
                return platformProperty.FindPropertyRelative("_androidSettings").FindPropertyRelative("_admobSettings");
            case PlatformSettingsType.Ios:
                return platformProperty.FindPropertyRelative("_iosSettings").FindPropertyRelative("_admobSettings");
            case PlatformSettingsType.YandexGames:
                return platformProperty.FindPropertyRelative("_yandexSettings");
            default:
                return null;
        }
    }

    private void ApplySettingsChanges()
    {
        _serializedObject.Update();
        EditorUtility.SetDirty(_settings);
    }

    private sealed class PlatformListRowElement : VisualElement
    {
        public PlatformListRowElement()
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.Center;
            style.paddingLeft = 4;
            style.paddingRight = 4;

            TitleLabel = new Label
            {
                style =
                {
                    flexGrow = 1,
                    unityTextAlign = TextAnchor.MiddleLeft,
                }
            };

            NameField = new TextField
            {
                isDelayed = true,
                style =
                {
                    flexGrow = 1,
                    marginTop = 0,
                    marginBottom = 0,
                    display = DisplayStyle.None,
                }
            };

            NameField.label = string.Empty;

            Add(TitleLabel);
            Add(NameField);
        }

        public Label TitleLabel { get; }
        public TextField NameField { get; }

        public void SetRenameMode(bool enabled)
        {
            TitleLabel.style.display = enabled ? DisplayStyle.None : DisplayStyle.Flex;
            NameField.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}

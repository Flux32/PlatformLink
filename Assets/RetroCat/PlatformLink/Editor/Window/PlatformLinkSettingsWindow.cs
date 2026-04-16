using System;
using System.Collections.Generic;
using RetroCat.PlatformLink.Editor.Modules;
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
    private VisualElement _settingsFooter;
    private Button _buildButton;

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
        _settingsFooter = rootVisualElement.Q<VisualElement>("settings-footer");
        _buildButton = rootVisualElement.Q<Button>("build-button");

        ConfigureAddPlatformMenu();
        ConfigurePlatformsList();
        ConfigureBuildButton();
        RefreshPlatforms();
        EnsureValidSelection();
        RenderSelectedPlatformSettings();
    }

    private void ConfigureBuildButton()
    {
        _buildButton.clicked += HandleBuildClicked;
        _buildButton.schedule.Execute(UpdateBuildButtonState).Every(500);
    }

    private void HandleBuildClicked()
    {
        int index = _platformsList.selectedIndex;
        if (index < 0 || index >= _platforms.Count)
            return;

        PlatformBuilder.Build(index);
    }

    private void UpdateBuildButtonState()
    {
        int index = _platformsList.selectedIndex;
        if (index < 0 || index >= _platforms.Count)
        {
            _settingsFooter.style.display = DisplayStyle.None;
            return;
        }

        PlatformSettingsType type = _platforms[index].Type;
        if (PlatformBuildTargetResolver.TryGetBuildTarget(type, out BuildTarget target) == false)
        {
            _settingsFooter.style.display = DisplayStyle.None;
            return;
        }

        _settingsFooter.style.display = DisplayStyle.Flex;

        bool isActive = EditorUserBuildSettings.activeBuildTarget == target;
        _buildButton.SetEnabled(isActive);
        _buildButton.tooltip = isActive
            ? $"Build the project for {target}."
            : $"Switch active Build Target to {target} to enable this build.";
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
        UpdateBuildButtonState();

        int index = _platformsList.selectedIndex;
        if (index < 0 || index >= _platforms.Count)
            return;

        _serializedObject.Update();
        _platformsProperty = _serializedObject.FindProperty("_platforms");

        SerializedProperty platformProperty = _platformsProperty.GetArrayElementAtIndex(index);
        SerializedProperty typeProperty = platformProperty.FindPropertyRelative("_type");
        PlatformSettingsType platformType = (PlatformSettingsType)typeProperty.enumValueIndex;

        RenderModulesSection(index, platformProperty, platformType);

        SerializedProperty settingsProperty = GetSettingsProperty(platformProperty, platformType);
        if (settingsProperty == null)
            return;

        PropertyField settingsField = new PropertyField(settingsProperty);
        settingsField.BindProperty(settingsProperty);
        _settingsContent.Add(settingsField);
        AddHelpBoxes(platformType);
    }

    private void RenderModulesSection(int entryIndex, SerializedProperty platformProperty, PlatformSettingsType platformType)
    {
        Foldout foldout = new Foldout { text = "Modules", value = true };
        foldout.style.marginBottom = 8;

        SerializedProperty modulesProperty = platformProperty.FindPropertyRelative("_modules");
        PlatformSettingsEntry entry = _platforms[entryIndex];

        for (int i = 0; i < modulesProperty.arraySize; i++)
        {
            SerializedProperty moduleProperty = modulesProperty.GetArrayElementAtIndex(i);
            SerializedProperty adapterConfigProperty = moduleProperty.FindPropertyRelative("_adapterConfig");
            if (i >= entry.Modules.Count)
                continue;

            foldout.Add(CreateModuleRow(entryIndex, i, entry.Modules[i], platformType, adapterConfigProperty));
        }

        Button addButton = new Button(() => ShowAddModuleMenu(entryIndex, platformType))
        {
            text = "+ Add Module",
        };
        addButton.style.marginTop = 4;
        foldout.Add(addButton);

        _settingsContent.Add(foldout);
    }

    private VisualElement CreateModuleRow(int entryIndex, int moduleIndex, PlatformModuleEntry moduleEntry, PlatformSettingsType platformType, SerializedProperty adapterConfigProperty)
    {
        VisualElement wrapper = new VisualElement
        {
            style =
            {
                marginTop = 2,
                marginBottom = 2,
                paddingTop = 4,
                paddingBottom = 4,
                paddingLeft = 6,
                paddingRight = 6,
                borderLeftWidth = 2,
                borderLeftColor = new Color(0.4f, 0.6f, 0.9f, 0.8f),
                backgroundColor = new Color(0, 0, 0, 0.08f),
            }
        };

        VisualElement header = new VisualElement
        {
            style = { flexDirection = FlexDirection.Row, alignItems = Align.Center }
        };

        Label kindLabel = new Label(ModuleAdapterRegistry.GetKindDisplayName(moduleEntry.Kind))
        {
            style = { flexGrow = 1, unityFontStyleAndWeight = FontStyle.Bold }
        };

        string adapterDisplay = moduleEntry.HasAdapter
            ? moduleEntry.AdapterConfig.DisplayName
            : "(none)";
        Button adapterButton = new Button(() => ShowAdapterMenu(entryIndex, moduleIndex, moduleEntry.Kind, platformType))
        {
            text = $"Adapter: {adapterDisplay} ▼",
            style = { marginRight = 4 }
        };

        Button removeButton = new Button(() => RemoveModule(entryIndex, moduleIndex))
        {
            text = "×",
            style = { width = 22 }
        };

        header.Add(kindLabel);
        header.Add(adapterButton);
        header.Add(removeButton);
        wrapper.Add(header);

        if (moduleEntry.HasAdapter)
        {
            PropertyField adapterField = new PropertyField(adapterConfigProperty, string.Empty);
            adapterField.BindProperty(adapterConfigProperty);
            wrapper.Add(adapterField);
        }

        return wrapper;
    }

    private void ShowAddModuleMenu(int entryIndex, PlatformSettingsType platformType)
    {
        GenericMenu menu = new GenericMenu();
        PlatformSettingsEntry entry = _settings.Platforms[entryIndex];
        IReadOnlyList<ModuleAdapterRegistry.AdapterDescriptor> adapters = ModuleAdapterRegistry.GetAdapters(platformType);

        foreach (PlatformModuleKind kind in Enum.GetValues(typeof(PlatformModuleKind)))
        {
            string label = ModuleAdapterRegistry.GetKindDisplayName(kind);

            if (entry.HasModuleOfKind(kind))
            {
                menu.AddDisabledItem(new GUIContent(label));
                continue;
            }

            bool hasAdaptersForPlatform = false;
            for (int i = 0; i < adapters.Count; i++)
            {
                if (adapters[i].Kind == kind)
                {
                    hasAdaptersForPlatform = true;
                    break;
                }
            }

            if (hasAdaptersForPlatform == false)
            {
                menu.AddDisabledItem(new GUIContent($"{label} (no adapters)"));
                continue;
            }

            PlatformModuleKind captured = kind;
            menu.AddItem(new GUIContent(label), false, () => AddModule(entryIndex, captured));
        }

        menu.ShowAsContext();
    }

    private void ShowAdapterMenu(int entryIndex, int moduleIndex, PlatformModuleKind kind, PlatformSettingsType platformType)
    {
        GenericMenu menu = new GenericMenu();
        PlatformModuleEntry moduleEntry = _settings.Platforms[entryIndex].Modules[moduleIndex];

        menu.AddItem(new GUIContent("None"), moduleEntry.HasAdapter == false, () => ClearAdapter(entryIndex, moduleIndex));
        menu.AddSeparator(string.Empty);

        IReadOnlyList<ModuleAdapterRegistry.AdapterDescriptor> adapters = ModuleAdapterRegistry.GetAdapters(platformType);
        bool anyAdded = false;
        for (int i = 0; i < adapters.Count; i++)
        {
            if (adapters[i].Kind != kind)
                continue;

            anyAdded = true;
            ModuleAdapterRegistry.AdapterDescriptor captured = adapters[i];
            bool isCurrent = moduleEntry.HasAdapter
                && moduleEntry.AdapterConfig.GetType() == captured.ConfigType;
            menu.AddItem(new GUIContent(captured.DisplayName), isCurrent, () => SetAdapter(entryIndex, moduleIndex, captured));
        }

        if (anyAdded == false)
            menu.AddDisabledItem(new GUIContent("No adapters available"));

        menu.ShowAsContext();
    }

    private void AddModule(int entryIndex, PlatformModuleKind kind)
    {
        if (entryIndex < 0 || entryIndex >= _settings.Platforms.Count)
            return;

        Undo.RecordObject(_settings, "Add Module");
        _settings.Platforms[entryIndex].AddModule(kind);
        ApplySettingsChanges();
        RefreshPlatforms();
        SetSelectedIndex(entryIndex);
    }

    private void SetAdapter(int entryIndex, int moduleIndex, ModuleAdapterRegistry.AdapterDescriptor adapter)
    {
        if (entryIndex < 0 || entryIndex >= _settings.Platforms.Count)
            return;

        Undo.RecordObject(_settings, "Set Adapter");
        _settings.Platforms[entryIndex].Modules[moduleIndex].SetAdapter(adapter.CreateConfig());
        ApplySettingsChanges();
        RefreshPlatforms();
        SetSelectedIndex(entryIndex);
    }

    private void ClearAdapter(int entryIndex, int moduleIndex)
    {
        if (entryIndex < 0 || entryIndex >= _settings.Platforms.Count)
            return;

        Undo.RecordObject(_settings, "Clear Adapter");
        _settings.Platforms[entryIndex].Modules[moduleIndex].ClearAdapter();
        ApplySettingsChanges();
        RefreshPlatforms();
        SetSelectedIndex(entryIndex);
    }

    private void RemoveModule(int entryIndex, int moduleIndex)
    {
        if (entryIndex < 0 || entryIndex >= _settings.Platforms.Count)
            return;

        Undo.RecordObject(_settings, "Remove Module");
        _settings.Platforms[entryIndex].RemoveModuleAt(moduleIndex);
        ApplySettingsChanges();
        RefreshPlatforms();
        SetSelectedIndex(entryIndex);
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

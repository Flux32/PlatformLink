using System;
using UnityEngine;

[Serializable]
public class EditorSettings
{
    [SerializeField] private EnvironmentEditorSettings _environment;
    [SerializeField] private StorageEditorSettings _storage;
    
    public EnvironmentEditorSettings Environment => _environment;
    public StorageEditorSettings Storage => _storage;
}

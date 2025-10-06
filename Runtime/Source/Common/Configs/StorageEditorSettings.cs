using System;
using UnityEngine;

[Serializable]
public class StorageEditorSettings
{
    public string SaveFilePath => _saveFilePath;
    
    [SerializeField] private string _saveFilePath;
}
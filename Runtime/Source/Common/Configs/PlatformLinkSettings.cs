using System;
using UnityEngine;

public class PlatformLinkSettings : ScriptableObject
{ 
    [SerializeField] private EditorSettings _editor;
    [SerializeField] private AndroidSettings _android;

    public AndroidSettings Android => _android;
    public EditorSettings Editor => _editor;

    private static PlatformLinkSettings s_instance;

    private const string ConfigFullPath = "Configs/PlatformLinkConfig";

    public static PlatformLinkSettings Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = Resources.Load<PlatformLinkSettings>(ConfigFullPath);

                if (s_instance == null)
                    throw new InvalidOperationException(); //TODO: exception message
            }

            return s_instance;
        }
    }
}
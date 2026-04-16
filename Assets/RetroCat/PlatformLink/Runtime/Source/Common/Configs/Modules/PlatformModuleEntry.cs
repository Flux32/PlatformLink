using System;
using UnityEngine;

[Serializable]
public class PlatformModuleEntry
{
    [SerializeField] private PlatformModuleKind _kind;
    [SerializeReference] private IModuleAdapterConfig _adapterConfig;

    public PlatformModuleKind Kind => _kind;
    public IModuleAdapterConfig AdapterConfig => _adapterConfig;
    public bool HasAdapter => _adapterConfig != null;

    public PlatformModuleEntry()
    {
    }

    public PlatformModuleEntry(PlatformModuleKind kind)
    {
        _kind = kind;
    }

    public void SetAdapter(IModuleAdapterConfig adapterConfig)
    {
        _adapterConfig = adapterConfig;
    }

    public void ClearAdapter()
    {
        _adapterConfig = null;
    }
}

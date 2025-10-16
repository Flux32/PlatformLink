using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using UnityEngine;

[Serializable]
public class EditorSettings
{
    [SerializeField] private EnvironmentEditorSettings _environment;
    [SerializeField] private StorageEditorSettings _storage;
    [SerializeField] private PurchasesEditorSettings _purchases;
    
    public EnvironmentEditorSettings Environment => _environment;
    public StorageEditorSettings Storage => _storage;
    public PurchasesEditorSettings Purchases => _purchases;
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
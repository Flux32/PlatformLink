using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    [Serializable]
    public class CatalogProduct
    {
        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public RemoteIcon ProductIcon { get; }
        public RemoteIcon CurrencyIcon { get; }
        public string Price { get; }
        public string PriceValue { get; }
        public string PriceCurrencyCode { get; }

        public CatalogProduct(
            string id,
            string title,
            string description,
            RemoteIcon productIcon,
            RemoteIcon currencyIcon,
            string price,
            string priceValue,
            string priceCurrencyCode)
        {
            Id = id;
            Title = title;
            Description = description;
            ProductIcon = productIcon;
            CurrencyIcon = currencyIcon;
            Price = price;
            PriceValue = priceValue;
            PriceCurrencyCode = priceCurrencyCode;
        }
    }
}

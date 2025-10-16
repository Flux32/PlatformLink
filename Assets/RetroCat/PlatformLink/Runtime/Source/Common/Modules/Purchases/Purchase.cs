namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    public class Purchase
    {
        public string ProductId { get; }
        public string Token { get; }
        public ProductType ProductType { get; }
        
        public Purchase(string productId, string token, ProductType productType)
        {
            ProductId = productId;
            ProductType = productType;
            Token = token; 
        }
    }
}
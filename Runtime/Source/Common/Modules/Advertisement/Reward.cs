namespace PlatformLink.Common
{
    public class Reward
    {
        public string Type { get; private set; }
        public double Amount { get; private set; }

        public Reward(string type, double amount) 
        { 
            Type = type;
            Amount = amount;
        }
    }
}

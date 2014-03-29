using System.Collections.Generic;

namespace SpacePewPew.FactoryMethod
{
    public class Factory
    {
        public struct FactoryItem
        {
            public Creator Creator { get; set; }
            public int Cost { get; set; }
            public string Name { get; set; }
        }

        public Dictionary<string, FactoryItem[]> Builder { get; set; }

        public Factory()
        {
            Builder = new Dictionary<string, FactoryItem[]>();
            // info
        }
    }
}

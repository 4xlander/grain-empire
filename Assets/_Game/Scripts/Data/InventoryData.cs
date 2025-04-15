using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class InventoryData
    {
        public Dictionary<string, int> Items = new();
    }
}

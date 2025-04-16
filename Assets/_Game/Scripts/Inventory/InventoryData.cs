using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class InventoryData
    {
        public List<InventoryItemData> Items = new();
    }
}

using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class GameData
    {
        public float Money;
        public List<FieldData> Fields = new();
        public List<BuildingData> Buildings = new();
        public InventoryData Inventory = new();

        public GameData(GameConfigSO config)
        {
            Money = config.StartMoney;
        }
    }
}

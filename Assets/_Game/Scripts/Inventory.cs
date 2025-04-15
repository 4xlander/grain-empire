namespace Game
{
    public class Inventory
    {
        private readonly InventoryData _data;

        public Inventory(GameData gameData)
        {
            _data = gameData.Inventory;
        }

        public void AddItem(string itemId, int count)
        {
            if (_data.Items.ContainsKey(itemId))
                _data.Items[itemId] += count;
            else
                _data.Items.Add(itemId, count);
        }

        public bool TryGetItemValue(string itemId, out int value ) =>
            _data.Items.TryGetValue(itemId, out value);
    }
}

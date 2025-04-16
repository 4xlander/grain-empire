using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class InventoryPresenter
    {
        private readonly Balance _balance;
        private readonly Inventory _inventory;
        private readonly InventoryUI _inventoryUI;
        private readonly Dictionary<string, InventoryItemConfigSO> _itemsConfigs;

        public InventoryPresenter(
            Balance balance,
            Inventory inventory,
            InventoryUI inventoryUI,
            IEnumerable<InventoryItemConfigSO> itemsConfigs)
        {
            _balance = balance;
            _inventory = inventory;
            _inventoryUI = inventoryUI;
            _itemsConfigs = itemsConfigs.ToDictionary(x => x.Id, x => x);

            _inventory.OnItemChanged += Inventory_OnItemChanged;
        }

        public void ShowInventory()
        {
            UpdateUI();
            _inventoryUI.Show();
        }

        private void UpdateUI()
        {
            _inventoryUI.Clear();

            var items = _inventory.GetAllItems();
            foreach (var item in items)
            {
                if (_itemsConfigs.TryGetValue(item.Id, out var config))
                    _inventoryUI.AddItem(new InventoryItemData
                    {
                        Id = item.Id,
                        Count = item.Count,
                    },
                    config.Icon,
                    config.Price,
                    () => SellItem(item.Id));
            }
        }

        private void SellItem(string itemId)
        {
            if (_inventory.TryGetItemValue(itemId, out var value))
                if (_itemsConfigs.TryGetValue(itemId, out var config))
                    if (_inventory.TryRemoveItem(itemId, value))
                        _balance.AddMoney(value * config.Price);
        }

        private void Inventory_OnItemChanged(object sender, Inventory.OnInventoryDataChanged e)
        {
            UpdateUI();
        }
    }
}

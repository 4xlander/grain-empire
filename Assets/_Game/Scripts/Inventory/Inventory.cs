using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Inventory
    {
        private readonly InventoryData _data;

        public event EventHandler<OnInventoryDataChanged> OnItemChanged;
        public class OnInventoryDataChanged : EventArgs
        {
            public string Item;
            public int Count;
        }

        public Inventory(InventoryData data)
        {
            _data = data;
        }

        public void AddItem(string itemId, int count)
        {
            InventoryItemData item;

            if (_data.Items.Exists(x => x.Id == itemId))
            {
                item = _data.Items.First(x => x.Id == itemId);
                item.Count += count;
            }
            else
            {
                item = new InventoryItemData()
                {
                    Id = itemId,
                    Count = count,
                };
                _data.Items.Add(item);
            }

            NotifyDataChanged(itemId, item.Count);
        }

        public bool TryGetItemValue(string itemId, out int value)
        {
            if (!_data.Items.Exists(x => x.Id == itemId))
            {
                value = 0;
                return false;
            }

            value = _data.Items.First(x => x.Id == itemId).Count;
            return true;
        }

        public bool TryRemoveItem(string itemId, int count)
        {
            if (!TryGetItemValue(itemId, out var value)
                || count > value) return false;

            var item = _data.Items.First(x => x.Id == itemId);
            item.Count -= count;
            NotifyDataChanged(itemId, item.Count);
            return true;
        }

        public List<InventoryItemData> GetAllItems() =>
            _data.Items;

        private void NotifyDataChanged(string itemId, int count)
        {
            OnItemChanged?.Invoke(this, new()
            {
                Item = itemId,
                Count = count,
            });
        }
    }
}

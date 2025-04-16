using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InventoryItemUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _totalCostText;

        [SerializeField] private Button _sellButton;

        public event Action OnSellButtonClick;

        private void Awake()
        {
            _sellButton.onClick.AddListener(() => OnSellButtonClick?.Invoke());
        }

        public void SetData(InventoryItemData data, Sprite icon, float price)
        {
            _icon.sprite = icon;
            _nameText.text = data.Id;
            _countText.text = data.Count.ToString();
            _priceText.text = $"Price: {price}";

            var totalCost = data.Count * price;
            _totalCostText.text = totalCost.ToString();
        }
    }
}

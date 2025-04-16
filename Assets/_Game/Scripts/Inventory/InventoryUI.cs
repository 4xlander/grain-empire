using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemUI _itemUITemplate;
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Hide);
        }

        private void Start()
        {
            _itemUITemplate.gameObject.SetActive(false);
            Hide();
        }

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        public void Clear()
        {
            foreach (Transform child in _container)
                if (child != _itemUITemplate.transform)
                    Destroy(child.gameObject);
        }

        public void AddItem(InventoryItemData itemData, Sprite icon, float price, Action onSellButtonClick)
        {
            var itemUI = Instantiate(_itemUITemplate, _container);
            itemUI.SetData(itemData, icon, price);
            itemUI.OnSellButtonClick += onSellButtonClick;
            itemUI.gameObject.SetActive(true);
        }
    }
}
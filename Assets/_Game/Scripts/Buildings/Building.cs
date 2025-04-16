using System;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Building : MonoBehaviour, IUpdatable
    {
        public event Action<Building> OnClick;

        [SerializeField] private int _plotId;
        [SerializeField] private GameObject _lockedVisual;
        [SerializeField] private TextMeshPro _priceText;
        [SerializeField] private Transform _buildingParent;
        [SerializeField] private BuildingConfigSO _buildingConfig;

        private bool _isBuilt;

        private void Start()
        {
            _priceText.text = $"${_buildingConfig.Price}";
        }

        public void OnUpdate(float deltaTime)
        {
            if (!gameObject.activeInHierarchy) return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.Equals(_lockedVisual))
                        OnClick?.Invoke(this);
                }
            }
        }

        public BuildingConfigSO GetBuilidngConfig() =>
            _buildingConfig;

        public void Build()
        {
            if (_isBuilt) return;
            _isBuilt = true;

            Instantiate(_buildingConfig.Prefab, _buildingParent);
            _lockedVisual.gameObject.SetActive(false);
        }

        public int GetPlotId() =>
            _plotId;
    }
}

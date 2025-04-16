using System;
using UnityEngine;

namespace Game
{
    public class Building : MonoBehaviour, IUpdatable
    {
        public event Action<Building> OnClick;

        [SerializeField] private int _plotId;
        [SerializeField] private GameObject _lockedVisual;
        [SerializeField] private Transform _buildingParent;
        [SerializeField] private BuildingConfigSO _buildingConfig;

        private Camera _mainCamera;
        private bool _isBuilt;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

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

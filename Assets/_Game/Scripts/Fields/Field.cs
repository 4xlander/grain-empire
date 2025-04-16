using System;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Field : MonoBehaviour, IUpdatable, IHasProgress
    {
        public event EventHandler OnClick;
        public event EventHandler OnStateChanged;
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

        [SerializeField] private int _plotId;
        [SerializeField] private FieldConfigSO _config;
        [SerializeField] private GameObject _lockedVisual;
        [SerializeField] private GameObject _unlockedVisual;
        [SerializeField] private TextMeshPro _priceText;

        [Space]
        [SerializeField] private GameObject _plowedVisual;
        [SerializeField] private GameObject _growStep1Visual;
        [SerializeField] private GameObject _growStep2Visual;
        [SerializeField] private GameObject _growStep3Visual;

        private const float GROW_STEP_1 = 0.3f;
        private const float GROW_STEP_2 = 0.6f;
        private const float GROW_STEP_3 = 1f;

        private FieldData _fieldData;
        private bool _isBuilt;
        private Inventory _inventory;

        public enum State
        {
            Idle,
            Plowing,
            Planting,
            Growing,
            HarvestReady,
            Gathering,
            Gathered,
        }

        public void Inject(FieldData fieldData, Inventory inventory)
        {
            _inventory = inventory;
            _fieldData = fieldData;
        }

        private void Start()
        {
            _priceText.text = $"${_config.Price}";
        }

        public void Build()
        {
            if (_isBuilt) return;
            _isBuilt = true;
        }

        public FieldConfigSO GetFieldConfig() => _config;

        public int GetPlotId() => _plotId;

        public State GetState() => _fieldData.State;

        public float GetProgress() => _fieldData.Progress;

        public void OnUpdate(float deltaTime)
        {
            if (!gameObject.activeInHierarchy) return;

            if (_isBuilt)
            {
                _unlockedVisual.SetActive(true);
                _lockedVisual.SetActive(false);

                UpdateState(deltaTime);
            }
            else
            {
                _unlockedVisual.SetActive(false);
                _lockedVisual.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider.gameObject.Equals(_lockedVisual))
                            OnClick?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void UpdateState(float deltaTime)
        {
            switch (_fieldData.State)
            {
                case State.Idle:
                    ResetVisual();
                    ChangeState(State.Plowing);
                    break;

                case State.Plowing:
                    var progress = _fieldData.Progress += deltaTime;
                    NotifyProgressChanged(progress / _config.PlowTime);

                    if (progress >= _config.PlowTime)
                    {
                        ResetProgress();
                        ChangeState(State.Planting);
                    }
                    break;

                case State.Planting:
                    _plowedVisual.SetActive(true);

                    progress = _fieldData.Progress += deltaTime;
                    NotifyProgressChanged(progress / _config.PlantTime);

                    if (progress >= _config.PlantTime)
                    {
                        ResetProgress();
                        ChangeState(State.Growing);
                    }
                    break;

                case State.Growing:
                    progress = _fieldData.Progress += deltaTime;
                    var progressNormalized = progress / _config.GrowthTime;

                    UpdateGrowingVisual(progressNormalized);
                    NotifyProgressChanged(progressNormalized);

                    if (progress >= _config.GrowthTime)
                    {
                        ResetProgress();
                        ChangeState(State.HarvestReady);
                    }
                    break;

                case State.HarvestReady:
                    ChangeState(State.Gathering);
                    break;

                case State.Gathering:
                    progress = _fieldData.Progress += deltaTime;
                    NotifyProgressChanged(progress / _config.GatherTime);

                    if (progress >= _config.GatherTime)
                    {
                        ResetProgress();
                        ChangeState(State.Gathered);
                    }
                    break;

                case State.Gathered:
                    _inventory.AddItem(_config.CropId, _config.Harvest);
                    ChangeState(State.Idle);
                    break;
            }
        }

        private void UpdateGrowingVisual(float progress)
        {
            if (progress >= GROW_STEP_1 && progress < GROW_STEP_2)
            {
                _growStep1Visual.SetActive(true);
                _growStep2Visual.SetActive(false);
                _growStep3Visual.SetActive(false);
            }

            if (progress >= GROW_STEP_2 && progress < GROW_STEP_3)
            {
                _growStep1Visual.SetActive(false);
                _growStep2Visual.SetActive(true);
                _growStep3Visual.SetActive(false);
            }

            if (progress >= GROW_STEP_3)
            {
                _growStep2Visual.SetActive(false);
                _growStep1Visual.SetActive(false);
                _growStep3Visual.SetActive(true);
            }
        }

        private void ResetVisual()
        {
            _plowedVisual.SetActive(false);
            _growStep1Visual.SetActive(false);
            _growStep2Visual.SetActive(false);
            _growStep3Visual.SetActive(false);
        }

        private void ResetProgress()
        {
            _fieldData.Progress = 0;
            NotifyProgressChanged(0);
        }

        private void ChangeState(State newState)
        {
            _fieldData.State = newState;
            NotifyStateChanged();
        }

        private void NotifyProgressChanged(float progressNormalized) =>
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                ProgressNormalized = progressNormalized
            });

        private void NotifyStateChanged() =>
            OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
}

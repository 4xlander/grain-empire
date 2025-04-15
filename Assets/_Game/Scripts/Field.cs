using System;
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

        private Camera _mainCamera;
        private FieldData _fieldData;
        private bool _isBuilt;

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

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public void Construct(FieldData fieldData)
        {
            _fieldData = fieldData;
        }

        public void OnUpdate(float deltaTime)
        {
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
                    Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

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
                    NotifyProgressChanged(progress / _config.GrowthTime);

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
                    break;
            }
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

        public FieldConfigSO GetFieldConfig() =>
            _config;

        public void Build()
        {
            if (_isBuilt) return;
            _isBuilt = true;
        }

        public int GetPlotId() =>
            _plotId;

        public State GetState() =>
            _fieldData.State;
    }
}

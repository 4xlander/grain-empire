using System;
using UnityEngine;

namespace Game
{
    public class FieldVisual : MonoBehaviour
    {
        [SerializeField] private Field _field;
        [SerializeField] private GameObject _plowedVisual;
        [SerializeField] private GameObject _growStep1Visual;
        [SerializeField] private GameObject _growStep2Visual;
        [SerializeField] private GameObject _growStep3Visual;

        private const float GROW_STEP_1 = 0.3f;
        private const float GROW_STEP_2 = 0.6f;
        private const float GROW_STEP_3 = 1f;

        private void OnEnable()
        {
            _field.OnStateChanged += Field_OnStateChanged;
            _field.OnProgressChanged += Field_OnProgressChanged;
        }

        private void Start()
        {
            UpdateState();
            UpdateProgress(_field.GetProgress());
        }

        private void OnDisable()
        {
            _field.OnStateChanged -= Field_OnStateChanged;
            _field.OnProgressChanged -= Field_OnProgressChanged;
        }

        private void Field_OnStateChanged(object sender, EventArgs e)
        {
            UpdateState();
        }

        private void Field_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
        {
            UpdateProgress(e.ProgressNormalized);
        }

        private void UpdateState()
        {
            switch (_field.GetState())
            {
                case Field.State.Idle:
                    _plowedVisual.SetActive(false);
                    _growStep1Visual.SetActive(false);
                    _growStep2Visual.SetActive(false);
                    _growStep3Visual.SetActive(false);
                    break;

                case Field.State.Planting:
                    _plowedVisual.SetActive(true);
                    _growStep1Visual.SetActive(false);
                    _growStep2Visual.SetActive(false);
                    _growStep3Visual.SetActive(false);
                    break;

                case Field.State.Gathered:
                    _plowedVisual.SetActive(false);
                    _growStep1Visual.SetActive(false);
                    _growStep2Visual.SetActive(false);
                    _growStep3Visual.SetActive(false);
                    break;
            }
        }

        private void UpdateProgress(float progress)
        {
            if (_field.GetState() != Field.State.Growing)
                return;

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
    }
}

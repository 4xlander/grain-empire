using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private GameObject _hasProgressGameObject;
        [SerializeField] private Image _barImage;

        private IHasProgress _hasProgress;

        private void Start()
        {
            _hasProgress = _hasProgressGameObject.GetComponent<IHasProgress>();
            if (_hasProgress != null)
                _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
            else
                Debug.LogError($"Game Object {_hasProgressGameObject} does not have a component that implements IHasProgress!");

            _barImage.fillAmount = 0;
            Hide();
        }

        private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
        {
            _barImage.fillAmount = e.ProgressNormalized;

            if (e.ProgressNormalized == 0 || e.ProgressNormalized == 1)
                Hide();
            else
                Show();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

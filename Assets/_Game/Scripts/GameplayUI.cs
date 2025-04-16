using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private Button _quitGameButton;
        [SerializeField] private GameObject _quitGameUI;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _stayButton;

        private Balance _balance;

        private void Awake()
        {
            _quitGameButton.onClick.AddListener(QuitMenu_OnClick);
            _quitButton.onClick.AddListener(QuitGame);
            _stayButton.onClick.AddListener(() => _quitGameUI.SetActive(false));
        }

        private void Start()
        {
            _quitGameUI.SetActive(false);
        }

        public void Inject(Balance balance)
        {
            _balance = balance;
            _balance.OnBalanceChanged += Balance_OnBalanceChanged;
            Balance_OnBalanceChanged(_balance.GetCurrentBalance());
        }

        private void Balance_OnBalanceChanged(float currentBalance)
        {
            _moneyText.text = _balance.GetCurrentBalance().ToString();
        }

        private void QuitMenu_OnClick()
        {
            _quitGameUI.SetActive(true);
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

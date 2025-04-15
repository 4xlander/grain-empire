using TMPro;
using UnityEngine;

namespace Game
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;

        private Balance _balance;

        public void Construct(Balance balance)
        {
            _balance = balance;
            _balance.OnBalanceChanged += Balance_OnBalanceChanged;
            Balance_OnBalanceChanged(_balance.GetCurrentBalance());
        }

        private void Balance_OnBalanceChanged(float currentBalance)
        {
            _moneyText.text = _balance.GetCurrentBalance().ToString();
        }
    }
}

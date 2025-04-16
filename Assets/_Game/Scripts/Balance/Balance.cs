using System;
using UnityEngine;

namespace Game
{
    public class Balance
    {
        private readonly GameData _data;

        public event Action<float> OnBalanceChanged;

        public Balance(GameData data)
        {
            _data = data;
        }

        public bool TrySpendMoney(float amount)
        {
            if (_data.Money >= amount)
            {
                _data.Money -= amount;
                Debug.Log($"Spent {amount} => Balance: {_data.Money}");

                OnBalanceChanged?.Invoke(_data.Money);
                return true;
            }
            else
            {
                Debug.Log($"Not enough money! Balance: {_data.Money}");
                return false;
            }
        }

        public void AddMoney(float amount)
        {
            _data.Money += amount;
            Debug.Log($"Received {amount} => Balance: {_data.Money}");

            OnBalanceChanged?.Invoke(_data.Money);
        }

        public float GetCurrentBalance() =>
            _data.Money;
    }
}

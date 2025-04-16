using System;
using UnityEngine;

namespace Game
{
    public class FarmUpgrades : MonoBehaviour
    {
        [Serializable]
        private class UpgradeTransforms
        {
            public int Level;
            public Transform[] Transforms;
        }

        [SerializeField] private UpgradeTransforms[] _upgrades;

        private FarmManager _manager;

        public void Inject(FarmManager manager)
        {
            _manager = manager;
            _manager.OnFarmLevelChanged += FarmManager_OnFarmLevelChanged;
        }

        private void Start()
        {
            CheckUpgrades(_manager.GetFarmLevel());
        }

        private void FarmManager_OnFarmLevelChanged(int newLevel)
        {
            CheckUpgrades(newLevel);
        }

        private void CheckUpgrades(int level)
        {
            foreach (var upgrade in _upgrades)
            {
                if (upgrade.Level <= level)
                    foreach (var t in upgrade.Transforms)
                        t.gameObject.SetActive(true);
                else
                    foreach (var t in upgrade.Transforms)
                        t.gameObject.SetActive(false);
            }
        }
    }
}

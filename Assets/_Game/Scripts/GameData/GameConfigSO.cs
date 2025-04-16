using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameConfigSO", menuName = "Game/Configs/Game Config")]
    public class GameConfigSO : ScriptableObject
    {
        [Tooltip("Time in seconds")]
        public float AutoSaveTime = 60f;
        public float StartMoney;
        public InventoryItemConfigSO[] InventoryItemsConfigs;
    }
}

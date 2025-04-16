using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "InventoryItemConfig", menuName = "Game/Configs/Inventory Item Config")]
    public class InventoryItemConfigSO : ScriptableObject
    {
        public string Id;
        public Sprite Icon;
        public float Price;
    }
}

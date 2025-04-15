using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Game/Configs/Builidng Config")]
    public class BuildingConfigSO : ScriptableObject
    {
        public string Id;
        public float Price;
        public Image Icon;
        public Transform Prefab;
    }
}

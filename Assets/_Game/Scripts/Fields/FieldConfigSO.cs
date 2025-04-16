using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "FieldConfigSO", menuName = "Game/Configs/Field Config")]
    public class FieldConfigSO : ScriptableObject
    {
        public string CropId;
        public float Price;
        public float PlowTime = 12f;
        public float PlantTime = 8f;
        public float GrowthTime = 30f;
        public float GatherTime = 20f;
        public int Harvest = 50;

        public GameObject Prefab;
    }
}

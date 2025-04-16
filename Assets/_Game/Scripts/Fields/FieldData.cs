using System;

namespace Game
{
    [Serializable]
    public class FieldData
    {
        public string CropId;
        public Field.State State;
        public float Progress;
        public int PlotId;
    }
}

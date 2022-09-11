using System;

namespace systems.audio_manager.ranged_float
{
    public class MinMaxRangeAttribute : Attribute
    {
        public MinMaxRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
        public float Min { get; private set; }
        public float Max { get; private set; }
    }
}
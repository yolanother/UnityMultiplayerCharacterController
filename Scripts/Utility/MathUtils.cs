using UnityEngine;

namespace DoubTech.MCC.Utilities
{
    public static class MathUtils
    {
        public static float Remap (this float value, float from1, float to1, float from2, float to2) {
            return Mathf.Clamp((value - from1) / (to1 - from1) * (to2 - from2) + from2, from2, to2);
        }
        
        public static float ValueApprox(this float value, float target, float threshold = .001f)
        {
            if (value < target + threshold && value > target - threshold) value = target;
            return value;
        }
    }
}
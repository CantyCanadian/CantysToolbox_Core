using UnityEngine;

namespace Canty
{
    public static class LayerMaskExtension
    {
        public static bool ContainsLayer(this LayerMask mask, int layer)
        {
            return ((1 << layer) & mask) != 0;
        }
    }
}
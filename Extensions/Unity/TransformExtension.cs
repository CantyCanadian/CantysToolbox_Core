using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEngine;

public static class TransformExtension
{
    /// <summary>
    /// Just like Transform.LookAt, but on a 2D axis, ignoring the Z axis.
    /// </summary>
    public static Transform LookAt2D(this Transform source, Vector2 target)
    {
        Vector2 dir = target - source.position.xy();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        source.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        return source;
    }
}

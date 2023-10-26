using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMarker : MonoBehaviour
{
    public float radius;
    public float angleOffset;
    public float spinSpeed;

    public void UpdateMarker(float ringAngle)
    {
        Vector2 pos;

        pos.x = radius * Mathf.Cos(ringAngle + angleOffset);
        pos.y = radius * Mathf.Sin(ringAngle + angleOffset);

        transform.localPosition = pos;
    }
}

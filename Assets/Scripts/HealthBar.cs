using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private AnimationClip animClip;
    [SerializeField] private AnimationCurve fillCurve;
    [SerializeField] [Range(0f, 1f)] private float healthRatio;

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateHealthBar();
    }
#endif

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float animTime = Mathf.Lerp(0f, animClip.length, fillCurve.Evaluate(healthRatio));
        animClip.SampleAnimation(gameObject, animTime);
    }
}

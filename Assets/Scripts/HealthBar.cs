using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private AnimationClip animClip;
    [SerializeField] private AnimationCurve fillCurve;
    [SerializeField] [Range(0f, 1f)] private float healthRatio;

    [Space]
    [SerializeField] private float healthChangeRate;

    private float healthTarget;

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateHealthBar();
    }
#endif

    private void Awake()
    {
        healthRatio = 1f;
        healthTarget = healthRatio;
        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();

        if (healthRatio > healthTarget)
            healthRatio = Mathf.Max(healthRatio - healthChangeRate * Time.deltaTime, healthTarget);
        if (healthRatio < healthTarget)
            healthRatio = Mathf.Min(healthRatio + healthChangeRate * Time.deltaTime, healthTarget);
    }

    private void UpdateHealthBar()
    {
        float animTime = Mathf.Lerp(0f, animClip.length, fillCurve.Evaluate(healthRatio));
        animClip.SampleAnimation(gameObject, animTime);
    }

    public void SetHealth(float ratio)
    {
        healthTarget = ratio;
    }
}

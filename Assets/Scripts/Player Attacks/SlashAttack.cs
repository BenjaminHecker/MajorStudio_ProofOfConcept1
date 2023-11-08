using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private TrailRenderer trail;

    private Animator anim;

    [Header("Config")]
    [SerializeField] private float damage;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        hurtbox.onHit += OnHit;
    }

    private void OnDestroy()
    {
        hurtbox.onHit -= OnHit;
    }

    public void Trigger()
    {
        anim.SetTrigger("Slash");
    }

    public void StartAttack()
    {
        hurtbox.active = true;
        trail.Clear();
        trail.enabled = true;
    }

    public void EndAttack()
    {
        hurtbox.active = false;
        trail.enabled = false;
    }

    private void OnHit()
    {
        hurtbox.active = false;
        PlayerController.AddRingMarker();
        EnemyController.TakeDamage(damage);
    }
}

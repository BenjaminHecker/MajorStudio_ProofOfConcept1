using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private TrailRenderer trail;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        hurtbox.gameObject.SetActive(false);
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
        hurtbox.gameObject.SetActive(true);
        trail.Clear();
        trail.enabled = true;
    }

    public void EndAttack()
    {
        hurtbox.gameObject.SetActive(false);
        trail.enabled = false;
    }

    private void OnHit()
    {
        hurtbox.gameObject.SetActive(false);
        PlayerController.HitEnemy();
    }
}

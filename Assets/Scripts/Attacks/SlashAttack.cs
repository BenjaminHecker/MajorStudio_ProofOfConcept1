using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    [SerializeField] private Hurtbox hurtbox;
    [SerializeField] private TrailRenderer trail;

    private Animator anim;

    private bool hitOpponent = true;

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
        hitOpponent = false;
        trail.Clear();
        anim.SetTrigger("Slash");
    }

    public void EnableTrail()
    {
        trail.enabled = true;
    }

    public void DisableTrail()
    {
        trail.enabled = false;
    }

    private void OnHit()
    {
        if (!hitOpponent)
        {
            hitOpponent = true;
            Debug.Log("Hit");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private string target;

    public delegate void Hit();
    public Hit onHit;

    [HideInInspector] public bool active = false;
    private bool collidingWithEnemy = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
            collidingWithEnemy = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
            collidingWithEnemy = false;
    }

    private void Update()
    {
        if (active && collidingWithEnemy)
            HitOpponent();
    }

    public void HitOpponent()
    {
        onHit?.Invoke();
    }
}

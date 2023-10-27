using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private string target;

    public delegate void Hit();
    public Hit onHit;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
            HitOpponent();
    }

    public void HitOpponent()
    {
        onHit?.Invoke();
    }
}

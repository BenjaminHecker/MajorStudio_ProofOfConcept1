using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public static TestEnemy instance;

    [SerializeField] private float damageAmount = 10f;

    private void Awake()
    {
        instance = this;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().Damage(transform.position, damageAmount);
        }
    }
}
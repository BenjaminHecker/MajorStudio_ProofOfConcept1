using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private static EnemyController instance;

    public static Vector3 Position { get { return instance.transform.position; } }

    private void Awake()
    {
        instance = this;
    }
}

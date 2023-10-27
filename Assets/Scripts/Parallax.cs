using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxFactor;

    private Transform cam;
    private Vector2 initialCamPos;
    private Vector2 initialObjectPos;

    private void Awake()
    {
        cam = Camera.main.transform;
        initialCamPos = cam.position;
        initialObjectPos = transform.position;
    }

    private void Update()
    {
        transform.position = initialObjectPos + ((Vector2) cam.position - initialCamPos) * parallaxFactor;
    }
}

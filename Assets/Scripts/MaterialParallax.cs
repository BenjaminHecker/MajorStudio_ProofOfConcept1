using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialParallax : MonoBehaviour
{
    [SerializeField] private float parallaxFactor = 0.5f;
    [SerializeField] private List<Material> materials = new List<Material>();

    private void Update()
    {
        Vector2 offset = transform.position * -parallaxFactor;

        foreach (Material mat in materials)
        {
            mat.SetVector("_Offset", offset);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialParallax : MonoBehaviour
{
    [SerializeField] private float parallaxFactor = 0.5f;
    [SerializeField] private List<MeshRenderer> meshes = new List<MeshRenderer>();

    private void Update()
    {
        Vector2 offset = transform.position * parallaxFactor;

        foreach (MeshRenderer mesh in meshes)
        {
            mesh.sharedMaterial.SetVector("Offset", offset);
        }
    }
}

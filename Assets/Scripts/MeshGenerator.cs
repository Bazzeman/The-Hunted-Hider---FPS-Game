using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        UpdateMesh(mesh,
            new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
                new Vector3(0.5F, 1, 0.5F),
                new Vector3(0.5F, -1, 0.5F)
            }, 
            new int[] {
                0, 1, 2,
                2, 1, 3,

                0, 4, 1,
                2, 4, 0,
                3, 4, 2,
                1, 4, 3,

                0, 5, 1,
                2, 5, 0,
                3, 5, 2,
                1, 5, 3
            });
    }

    void Update()
    {
        
    }

    void UpdateMesh(Mesh mesh, Vector3[] vertices, int[] triangles)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}

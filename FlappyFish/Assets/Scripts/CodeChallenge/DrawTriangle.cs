using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class DrawTriangle : MonoBehaviour
{
    Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        MakeMeshData();
        CreateMesh();
    }


    void MakeMeshData()
    {
        verticies = new Vector3[] { new Vector3(-100, 0, 0), new Vector3(0, 100, 0), new Vector3(100, 0, 0) };
        triangles = new int[] { 0, 1, 2 };

    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Transform carTransform;
    public MeshFilter carMeshFilter;

    Mesh carMesh;
    Vector3[] baseCarVertices;
    Vector3[] currentCarVertices;

    void Start()
    {
        carMesh = carMeshFilter.mesh;
        baseCarVertices = carMesh.vertices;
        currentCarVertices = new Vector3[baseCarVertices.Length];
        for (int i = 0; i < baseCarVertices.Length; i++)
        {
            currentCarVertices[i] = baseCarVertices[i];
        }
    }

    void Update()
    {
        Matrix4x4 carMatrix = carTransform.localToWorldMatrix;

        for (int i = 0; i < currentCarVertices.Length; i++)
        {
            Vector4 temp = new Vector4(baseCarVertices[i].x, baseCarVertices[i].y, baseCarVertices[i].z, 1);
            currentCarVertices[i] = carMatrix * temp;
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = currentCarVertices;
        mesh.RecalculateNormals();
    }
}

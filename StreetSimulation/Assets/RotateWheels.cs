using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMatrices : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Axis around which rotation will occur
    public float rotationSpeed = 50.0f; // Speed of rotation

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] rotatedVertices;

    private float currentRotationAngle = 0.0f;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        rotatedVertices = new Vector3[originalVertices.Length];
        originalVertices.CopyTo(rotatedVertices, 0);
    }

    void Update()
    {
        // Increment the rotation angle based on speed and time
        float angle = rotationSpeed * Time.deltaTime;
        currentRotationAngle += angle;

        // Create rotation matrix
        Quaternion rotation = Quaternion.AngleAxis(currentRotationAngle, rotationAxis);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

        // Apply rotation to vertices
        for (int i = 0; i < originalVertices.Length; i++)
        {
            rotatedVertices[i] = rotationMatrix.MultiplyPoint3x4(originalVertices[i]);
        }

        // Update mesh with rotated vertices
        mesh.vertices = rotatedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}


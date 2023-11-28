/*
Functions to work with transformation matrices in 3D

Gilberto Echeverria
2022-11-03
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTransforms : MonoBehaviour
{
    [SerializeField] Vector3 displacement;
    [SerializeField] GameObject[] wheels;


    Mesh mesh;
    Vector3[] baseVertices;
    Vector3[] newVertices;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        baseVertices = mesh.vertices;


        // Allocate mmeory for the copy of the vector list
        newVertices = new Vector3[baseVertices.Length];
        // Copy the coordinates
        for (int i = 0; i < baseVertices.Length; i++) {
            newVertices[i] = baseVertices[i];
        }

        DoTransform();
    }

    // Update is called once per frame
    void Update() 
    {
        displacement.x += 1.0f * Time.deltaTime;
        TranslateWheels(displacement.x);
        TranslateWheels(displacement.y);
        TranslateWheels(displacement.z);

    }

    void TranslateWheels(float howMuch) {
        foreach (GameObject wheel in wheels) {
            mesh = wheel.GetComponentInChildren<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            Matrix4x4 move = HW_Transforms.TranslationMat(howMuch, 0.0f, 0.0f);
            for (int i = 0; i < vertices.Length; i++) {
                Vector4 temp = new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, 1.0f);
                temp = move * temp;
                vertices[i] = new Vector3(temp.x, temp.y, temp.z);
            }

            mesh.vertices = vertices;
            // recalcular las nrmales
            mesh.RecalculateNormals();
        }

    }

    void DoTransform() 
    {
        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x, 
                                                      displacement.y, 
                                                      displacement.z);   

        for (int i = 0; i < newVertices.Length; i++) {
            Vector4 temp = new Vector4(baseVertices[i].x, 
                                       baseVertices[i].y, 
                                       baseVertices[i].z, 
                                       1.0f);
        }  
    }

    void MoveOrigin() {
        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x, 
                                                      displacement.y, 
                                                      displacement.z);   
        for (int i = 0; i < newVertices.Length; i++) {
            Vector4 temp = new Vector4(baseVertices[i].x, 
                                       baseVertices[i].y, 
                                       baseVertices[i].z, 
                                       1.0f);
            temp = move * temp;
            newVertices[i] = new Vector3(temp.x, temp.y, temp.z);
        }
    }
}

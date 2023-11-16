// /*
// Use transformation matrices to modify the vertices of a mesh

// David Vieyra
// 2023-11-15
// */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ApplyTransforms : MonoBehaviour
// {
//     [SerializeField] Vector3 displacement;
//     [SerializeField]float angle;
//     [SerializeField] AXIS rotationAxis;
//     [SerializeField] GameObject Car;
//     Mesh mesh;
//     Vector3[] baseVertices;
//     Vector3[] newVertices;

//     [SerializeField] GameObject[] WheelObjects;

//     // Start is called before the first frame update
//     void Start()
//     {
//         mesh = GetComponentInChildren<MeshFilter>().mesh;
//         baseVertices = mesh.vertices;

//         newVertices = new Vector3[baseVertices.Length];
//         for (int i = 0; i < baseVertices.Length; i++)
//         {
//             newVertices[i] = baseVertices[i];
//         }
        
        
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         DoTransform();

//     }

//     void DoTransform(){
        
//         //create the matrices
//         Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x *Time.time , displacement.y *Time.time, displacement.z *Time.time);
//         // Matrix4x4 moveOrigin = HW_Transforms.TranslationMat(-displacement.x, -displacement.y, -displacement.z);
//         // Matrix4x4 moveObject = HW_Transforms.TranslationMat(displacement.x, displacement.y, displacement.z);
//         Matrix4x4 rotate = HW_Transforms.RotateMat(angle * Time.time, rotationAxis);

//         Matrix4x4 composite = rotate * move;

//         for (int i=0; i<newVertices.Length; i++)
//         {
//             Vector4 temp = new Vector4(baseVertices[i].x, baseVertices[i].y, baseVertices[i].z, 1);

//             newVertices[i] = composite * temp;
//         }
        

//         mesh.vertices = newVertices;
//         mesh.RecalculateNormals();
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTransforms : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] AXIS rotationAxis = AXIS.Z; // Set default rotation axis to Z
    [SerializeField] GameObject[] WheelObjects; // Array of wheel game objects
    Mesh[] meshes;
    Vector3[][] baseVertices;
    Vector3[][] newVertices;
    Vector3[] wheelCenters;

    // Start is called before the first frame update
    void Start()
    {
        meshes = new Mesh[WheelObjects.Length];
        baseVertices = new Vector3[WheelObjects.Length][];
        newVertices = new Vector3[WheelObjects.Length][];
        wheelCenters = new Vector3[WheelObjects.Length];

        for (int i = 0; i < WheelObjects.Length; i++)
        {
            meshes[i] = WheelObjects[i].GetComponentInChildren<MeshFilter>().mesh;
            baseVertices[i] = meshes[i].vertices;
            newVertices[i] = new Vector3[baseVertices[i].Length];

            // Calculate wheel center in local space
            Vector3 center = meshes[i].bounds.center;
            wheelCenters[i] = WheelObjects[i].transform.InverseTransformPoint(center);

            for (int j = 0; j < baseVertices[i].Length; j++)
            {
                newVertices[i][j] = baseVertices[i][j];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DoTransform();
    }

    void DoTransform()
    {
        for (int i = 0; i < WheelObjects.Length; i++)
        {
            // Create the matrices
            Matrix4x4 move = HW_Transforms.TranslationMat(wheelCenters[i].x, wheelCenters[i].y, wheelCenters[i].z);
            Matrix4x4 moveBack = HW_Transforms.TranslationMat(-wheelCenters[i].x, -wheelCenters[i].y, -wheelCenters[i].z);
            Matrix4x4 rotate = HW_Transforms.RotateMat(angle * Time.time, rotationAxis);

            Matrix4x4 composite = moveBack * rotate * move;

            for (int j = 0; j < newVertices[i].Length; j++)
            {
                Vector4 temp = new Vector4(baseVertices[i][j].x, baseVertices[i][j].y, baseVertices[i][j].z, 1);
                newVertices[i][j] = composite * temp;
            }

            meshes[i].vertices = newVertices[i];
            meshes[i].RecalculateNormals();
        }
    }
}


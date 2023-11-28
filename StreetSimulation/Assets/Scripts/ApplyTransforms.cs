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
//     // [SerializeField] Vector3 displacement;
//     [SerializeField]float angle;
//     [SerializeField] AXIS rotationAxis;
//     [SerializeField] GameObject Car;
//     Mesh mesh;
//     Vector3[] baseVertices;
//     Vector3[] newVertices;

//     [SerializeField] GameObject[] WheelObjects;

//     MoveCar moveCarScript; // Reference to the ScriptA instance

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
//         // Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x *Time.time , displacement.y *Time.time, displacement.z *Time.time);
//         // Matrix4x4 moveOrigin = HW_Transforms.TranslationMat(-displacement.x, -displacement.y, -displacement.z);
//         // Matrix4x4 moveObject = HW_Transforms.TranslationMat(displacement.x, displacement.y, displacement.z);
//         Matrix4x4 rotate = HW_Transforms.RotateMat(angle * Time.time, rotationAxis);

//         Matrix4x4 composite = rotate;

//         for (int i=0; i<newVertices.Length; i++)
//         {
//             Vector4 temp = new Vector4(baseVertices[i].x, baseVertices[i].y, baseVertices[i].z, 1);

//             newVertices[i] = composite * temp;
//         }
        

//         mesh.vertices = newVertices;
//         mesh.RecalculateNormals();
//     }
// }
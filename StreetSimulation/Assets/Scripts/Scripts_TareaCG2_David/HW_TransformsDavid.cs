// /*
// Functions to work with transformation matrices in 3D

// Gilberto Echeverria
// 2022-11-03
// */


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // Enumeration to define the axis
// public enum AXISFer {X, Y, Z};
// // Values:        0  1  2

// public class HW_TransformsFer : MonoBehaviour
// {
//     public static Matrix4x4 TranslationMat(float tx, float ty, float tz)
//     {
//         Matrix4x4 matrix = Matrix4x4.identity;
//         matrix[0, 3] = tx;
//         matrix[1, 3] = ty;
//         matrix[2, 3] = tz;
//         return matrix;
//     }

//     public static Matrix4x4 ScaleMat(float sx, float sy, float sz)
//     {
//         Matrix4x4 matrix = Matrix4x4.identity;
//         matrix[0, 0] = sx;
//         matrix[1, 1] = sy;
//         matrix[2, 2] = sz;
//         return matrix;
//     }

//     public static Matrix4x4 RotateMat(float angle, AXISFer axis)
//     {
//         float rads = angle * Mathf.Deg2Rad;
//         float cosTheta = Mathf.Cos(rads);
//         float sinTheta = Mathf.Sin(rads);
//         Mathf.Sin(rads);

//         Matrix4x4 matrix = Matrix4x4.identity;
//         if (axis == AXISFer.X) {
//             matrix[1,1] = cosTheta;
//             matrix[1,2] = sinTheta;
//             matrix[2,1] = -sinTheta;
//             matrix[2,2] = cosTheta;

//         } else if (axis == AXISFer.Y) {
//             matrix[0,0] = cosTheta;
//             matrix[0,2] = -sinTheta;
//             matrix[2,0] = sinTheta;
//             matrix[2,2] = cosTheta;

//         } else if (axis == AXISFer.Z) {
//             matrix[0,0] = cosTheta;
//             matrix[0,1] = sinTheta;
//             matrix[1,0] = -sinTheta;
//             matrix[1,1] = cosTheta;
//         }
//         return matrix;
//     }

//     public static Matrix4x4 RotateMat(float angle, Vector3 axis)
//     {
//         float rads = angle * Mathf.Deg2Rad;
//         float cosTheta = Mathf.Cos(rads);
//         float sinTheta = Mathf.Sin(rads);

//         Matrix4x4 matrix = Matrix4x4.identity;

//         axis.Normalize();

//         float ux = axis.x;
//         float uy = axis.y;
//         float uz = axis.z;

//         matrix[0, 0] = cosTheta + (1 - cosTheta) * ux * ux;
//         matrix[0, 1] = (1 - cosTheta) * ux * uy - sinTheta * uz;
//         matrix[0, 2] = (1 - cosTheta) * ux * uz + sinTheta * uy;

//         matrix[1, 0] = (1 - cosTheta) * uy * ux + sinTheta * uz;
//         matrix[1, 1] = cosTheta + (1 - cosTheta) * uy * uy;
//         matrix[1, 2] = (1 - cosTheta) * uy * uz - sinTheta * ux;

//         matrix[2, 0] = (1 - cosTheta) * uz * ux - sinTheta * uy;
//         matrix[2, 1] = (1 - cosTheta) * uz * uy + sinTheta * ux;
//         matrix[2, 2] = cosTheta + (1 - cosTheta) * uz * uz;

//         return matrix;
//     }

    
// }
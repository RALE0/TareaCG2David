
// David Santiago Vieyra García A01656030
// Tarea CG2 Movimiento del carro y las llantas con matrices de transformación. 
// De acuerdo a la orientación del desplazamiento del carro, se calcula el ángulo de rotación del carro y de las llantas. 
// Haciendo que en todo momento el carro oriente su frente hacia el movimiento y las llantas giren en el eje Z. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTransformations : MonoBehaviour
{
    [SerializeField] Vector3 movementDirection; // Direction of car movement
    private float wheelSpinSpeed; // Speed of wheel spin
    [SerializeField] GameObject[] wheels; // Array to assign wheels in the inspector

    Transform carTransform; // Car's transform
    float rotationAngle; // Rotation angle of the car
    Mesh[] carMeshes; // Array to store car and wheel meshes
    Vector3[][] baseVertices; // Array to store base vertices of meshes
    Vector3[][] transformedVertices; // Array to store transformed vertices of meshes

    // Start is called before the first frame update
    void Start()
    {
        if (wheels.Length != 4)
        {
            Debug.LogError("Four wheels must be assigned!");
            return;
        }

        carTransform = gameObject.transform;

        carMeshes = new Mesh[5];
        baseVertices = new Vector3[5][];
        transformedVertices = new Vector3[5][];

        carMeshes[0] = GetComponentInChildren<MeshFilter>().mesh;
        for (int i = 0; i < wheels.Length; i++)
        {
            carMeshes[i + 1] = wheels[i].GetComponentInChildren<MeshFilter>().mesh;
        }

        for (int i = 0; i < carMeshes.Length; i++)
        {
            baseVertices[i] = carMeshes[i].vertices;
            transformedVertices[i] = new Vector3[baseVertices[i].Length];
            for (int j = 0; j < baseVertices[i].Length; j++)
            {
                transformedVertices[i][j] = baseVertices[i][j];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the car's speed per frame based on movement direction
        float carSpeed = movementDirection.magnitude;
        Debug.Log("Car speed: " + carSpeed);

        // Calculate the spin speed based on the car's speed and wheel spin radius
        // Convert linear speed to angular velocity: v = r * omega
        wheelSpinSpeed = carSpeed * 10000; // This is in radians per frame
        Debug.Log("Wheel spin speed: " + wheelSpinSpeed);

        ApplyTransformations();
    }

    void ApplyTransformations()
    {
        // Calculate the rotation angle of the car based on the movement direction
        rotationAngle = Mathf.Atan2(movementDirection.z, movementDirection.x) * Mathf.Rad2Deg;

        // Transformation matrix for movement
        Matrix4x4 translationMatrix = HW_Transforms.TranslationMat(movementDirection.x * Time.time,
                                                                  movementDirection.y * Time.time,
                                                                  movementDirection.z * Time.time);

        // Transformation matrix for car rotation
        Matrix4x4 rotationMatrix = HW_Transforms.RotateMat(rotationAngle, AXIS.Y);

        // Transformation matrix for wheel spin along the Z-axis
        Matrix4x4 wheelSpinMatrix = HW_Transforms.RotateMat(wheelSpinSpeed * Time.time, AXIS.Z);

        for (int i = 0; i < carMeshes.Length; i++)
        {
            Matrix4x4 compositeMatrix;

            if (i > 0)
            {
                // Calculate the relative position of the wheel with respect to the car
                Vector3 wheelPositionRelativeToCar = carTransform.InverseTransformPoint(wheels[i - 1].transform.position);

                // Transformation matrix to bring the wheel to the origin
                Matrix4x4 moveToOriginMatrix = HW_Transforms.TranslationMat(-wheelPositionRelativeToCar.x,
                                                                          -wheelPositionRelativeToCar.y,
                                                                          -wheelPositionRelativeToCar.z);

                // Transformation matrix to return the wheel to its original position
                Matrix4x4 returnToOriginalPositionMatrix = HW_Transforms.TranslationMat(wheelPositionRelativeToCar.x,
                                                                                      wheelPositionRelativeToCar.y,
                                                                                      wheelPositionRelativeToCar.z);

                // Combine the transformations
                compositeMatrix = translationMatrix * moveToOriginMatrix * rotationMatrix * returnToOriginalPositionMatrix * wheelSpinMatrix;
            }
            else
            {
                // Apply transformations for the car body (not wheels)
                compositeMatrix = translationMatrix * rotationMatrix;
            }

            for (int j = 0; j < baseVertices[i].Length; j++)
            {
                Vector4 tempVector = new Vector4(baseVertices[i][j].x,
                                                baseVertices[i][j].y,
                                                baseVertices[i][j].z,
                                                1);

                // Apply the composite transformation to the vertices
                transformedVertices[i][j] = compositeMatrix * tempVector;
            }

            // Update the mesh with the transformed vertices
            carMeshes[i].vertices = transformedVertices[i];
            carMeshes[i].RecalculateNormals();
            carMeshes[i].RecalculateBounds();
        }
    }
}

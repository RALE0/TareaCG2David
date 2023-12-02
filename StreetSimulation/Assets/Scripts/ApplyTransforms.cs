// Programa para aplicar matrices de transformacion a un coche y sus llantas
// Fernanda Cantú Ortega - A01782232
// 15 de noviembre de 2023
// Corregido: 1 de diciembre de 2023

// Líbrerias a utilizar
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase para aplicar transformaciones
public class ApplyTransforms : MonoBehaviour
{
    // Definir variables
    [SerializeField] Vector3 displacement;
    [SerializeField] float rotationSpeed;
    [SerializeField] AXIS rotationAxis;

    [SerializeField] GameObject[] car;
    Mesh carMesh;
    Vector3[] car_baseVertices;
    Vector3[] car_newVertices;

    [SerializeField] GameObject[] wheels;
    Mesh wheelMesh;
    Vector3[][] wheel_baseVertices;
    Vector3[][] wheel_newVertices;

    // Función para inicializar
    public void Start()
    {
        // Para los coches:
        // Conseguir mesh y vertices del coche, y copiar vertices originales
        carMesh = GetComponentInChildren<MeshFilter>().mesh;
        if (carMesh == null)
        {
            Debug.LogError("Mesh del coche no encontrado");
        }
        else
        {
            Debug.Log("Mesh del coche conseguido");
        }
        car_baseVertices = carMesh.vertices;
        car_newVertices = new Vector3[car_baseVertices.Length];

        // Recorrer vertices base y guardarlos en los nuevos
        for (int i = 0; i < car_baseVertices.Length; i++) 
        {
            car_newVertices[i] = car_baseVertices[i];
            Debug.Log("Vertices del coche copiados");
        }

        // Para las llantas:
        // Obtener mesh y guardar vertices de cada llanta
        wheel_baseVertices = new Vector3[wheels.Length][];
        for (int i = 0; i < wheels.Length; i++) 
        {
            wheelMesh = wheels[i].GetComponentInChildren<MeshFilter>().mesh;  
            if (wheelMesh == null)
            {
                Debug.LogError("Mesh de llanta no encontrado");
            }
            else
            {
                Debug.Log("Mesh de llanta conseguido");
            }
            wheel_baseVertices[i] = wheelMesh.vertices;
        }

        // Copiar vertices originales de las llantas
        wheel_newVertices = new Vector3[wheels.Length][];
        for (int i = 0; i < wheels.Length; i++) 
        {
            wheel_newVertices[i] = new Vector3[wheel_baseVertices[i].Length];
            
            for (int j = 0; j < wheel_baseVertices[i].Length; j++) 
            {
                wheel_newVertices[i][j] = wheel_baseVertices[i][j];
                Debug.Log("Vertices de llanta copiados");
            }
        }
    }

    // Función que se llama cada frame
    public void Update()
    {
        DoTransform();
        Debug.Log("Llamar a DoTransform");
    }

    // Función para aplicar transformaciones
    public void DoTransform()
    {
        // Transformaciones al coche:
        // Matriz para trasladar un objeto
        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x * Time.time,
                                                        displacement.y * Time.time,
                                                        displacement.z * Time.time);

        Debug.Log("Matriz de traslación conseguida");
        // Multiplicar cada vertice del coche por la matriz de traslación
        for (int i = 0; i < car_newVertices.Length; i++)
        {
            Debug.Log("Multiplicar vertices del coche por matriz de traslación");
            Vector4 temp = new Vector4(car_baseVertices[i].x,
                                       car_baseVertices[i].y,
                                       car_baseVertices[i].z,
                                       1);

            car_newVertices[i] = move * temp; 
        }
        
        carMesh.vertices = car_newVertices;
        Debug.Log("Vertices del coche actualizados");
        carMesh.RecalculateNormals();
        Debug.Log("Normales del coche actualizadas");

        // Transformaciones a las llantas:
        // Ciclo para recorrer todas las llantas
        for (int i = 0; i < wheels.Length; i++)
        {
            // Matriz para rotar un objeto
            Matrix4x4 rotate = HW_Transforms.RotateMat(rotationSpeed * Time.time, rotationAxis);
            
            // Matriz para moverse y rodar
            Matrix4x4 composite = move * rotate;

            // Multiplicar cada vertice de las llantas por la matriz compuesta
            for (int j = 0; j < wheel_newVertices[i].Length; j++)
            {
                Vector4 temp = new Vector4(wheel_baseVertices[i][j].x,
                                           wheel_baseVertices[i][j].y,
                                           wheel_baseVertices[i][j].z,
                                           1);

                wheel_newVertices[i][j] = composite * temp;
            }

            wheels[i].GetComponentInChildren<MeshFilter>().mesh.vertices = wheel_newVertices[i];
            wheels[i].GetComponentInChildren<MeshFilter>().mesh.RecalculateNormals();
        }
    }
}


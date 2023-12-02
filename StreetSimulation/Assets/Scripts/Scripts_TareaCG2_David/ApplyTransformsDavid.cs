using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour
{
    [SerializeField] float currentSpeed = 0.1f;
    [SerializeField] float angle = 1f;
    public GameObject[] WheelObjects;  // Reference to wheel game objects

    [SerializeField] Vector3 displacement;

    [SerializeField]AXIS rotationAxis;

    Mesh[] wheelMesh;
    Vector3[] wheelVertices;

    

    Mesh mesh;
    Vector3[] baseVertices;
    Vector3[] newVertices;

    Vector3 lastPosition;

    public float rotationSpeed = 90f;

    void Start()
    {
        InitializeWheels();
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        baseVertices = mesh.vertices;

        newVertices = new Vector3[baseVertices.Length];
        for (int i = 0; i < baseVertices.Length; i++)
        {
            newVertices[i] = baseVertices[i];
        }

        lastPosition = transform.position;
        
    }

    void Update()
    {
        float translationAmount = currentSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            MoveCarFBObject(translationAmount);
            MoveWheelsandRotate(translationAmount);
        }

        if (Input.GetKey(KeyCode.S))
        {
            MoveCarFBObject(-translationAmount);
            MoveWheelsandRotate(-translationAmount);
        }

        
        // Function to move the car sideways and make it look towards the side it is moving in the y axis with matrix application 
        if (Input.GetKey(KeyCode.A))
        {
            MoveCarLRObject(translationAmount);
            MoveWheelsLR(translationAmount);  
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveCarLRObject(-translationAmount);
            MoveWheelsLR(-translationAmount);
        }

        mesh.vertices = newVertices;
        mesh.RecalculateNormals();

        lastPosition = transform.position;
    }

    void InitializeWheels()
    {
        foreach (GameObject wheelObject in WheelObjects)
        {
            Mesh wheelMesh = wheelObject.GetComponentInChildren<MeshFilter>().mesh;
            // Get the wheel's mesh and vertices, do the initialization here
            Vector3[] wheelVertices = wheelMesh.vertices; // 
        }
    }

    bool hasRotatedForward = false; // Keep track of forward rotation
    bool hasRotatedBackward = false; // Keep track of backward rotation

    void MoveCarFBObject(float translationAmount)
    {
        // Calculate translation matrix for forward/backward movement
        Matrix4x4 move = HW_TransformsFer.TranslationMat(translationAmount, 0f, 0f);

        // Apply the translation to the car
        ApplyTransformation(move);

    }

    bool hasRotated = false; // Keep track of whether rotation has occurred

    void MoveCarLRObject(float translationAmount)
    {
        Matrix4x4 move = HW_TransformsFer.TranslationMat(0f, 0f, translationAmount);
        ApplyTransformation(move);
    }


    
    void MoveWheelsLR(float translationAmount)
    {
        foreach (GameObject wheelObject in WheelObjects)
        {
            Mesh wheelMesh = wheelObject.GetComponentInChildren<MeshFilter>().mesh;
            Vector3[] wheelVertices = wheelMesh.vertices;
            Matrix4x4 move = HW_TransformsFer.TranslationMat(0f, 0f, translationAmount);
            Matrix4x4 rotate = HW_TransformsFer.RotateMat(angle * Time.time, AXISFer.Z);
            Matrix4x4 composite = move * rotate;

            for (int i = 0; i < wheelVertices.Length; i++)
            {
                Vector4 temp = new Vector4(wheelVertices[i].x, wheelVertices[i].y, wheelVertices[i].z, 1);
                wheelVertices[i] = move * temp;
            }

            wheelMesh.vertices = wheelVertices;
            wheelMesh.RecalculateNormals();
        }
    }



    void ApplyTransformation(Matrix4x4 transformationMatrix)
    {
        for (int i = 0; i < newVertices.Length; i++)
        {
            Vector4 temp = new Vector4(newVertices[i].x, newVertices[i].y, newVertices[i].z, 1);
            newVertices[i] = transformationMatrix * temp;
        }
    }

    void MoveWheelsandRotate(float translationAmount)
    {
        foreach (GameObject wheelObject in WheelObjects)
        {
            Mesh wheelMesh = wheelObject.GetComponentInChildren<MeshFilter>().mesh;
            Vector3[] wheelVertices = wheelMesh.vertices;

            Matrix4x4 move = HW_TransformsFer.TranslationMat(translationAmount, 0f, 0f);
            Matrix4x4 rotate = HW_TransformsFer.RotateMat(angle * Time.time, AXISFer.Z);

            Matrix4x4 composite = move;

            for (int i = 0; i < wheelVertices.Length; i++)
            {
                Vector4 temp = new Vector4(wheelVertices[i].x, wheelVertices[i].y, wheelVertices[i].z, 1);
                wheelVertices[i] = composite * temp;
            }

            wheelMesh.vertices = wheelVertices;
            wheelMesh.RecalculateNormals();
        }
    }
}

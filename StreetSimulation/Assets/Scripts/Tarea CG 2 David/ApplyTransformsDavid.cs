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

    void Start()
    {
        // InitializeWheels();
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
            foreach (GameObject wheelObject in WheelObjects)
            {
                MoveWheelsandRotate(translationAmount);
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            MoveCarFBObject(-translationAmount);
            MoveWheelsandRotate(-translationAmount);
            // MoveWheelsOrigin(-translationAmount);
            //RotateWheels();
        }

        
        // Function to move the car sideways and make it look towards the side it is moving in the y axis with matrix application 
        if (Input.GetKey(KeyCode.A))
        {
            MoveCarLRObject(translationAmount);
            MoveWheelsLR(translationAmount);

            // Matrix4x4 rotate = HW_TransformsFer.RotateMat(angle, AXISFer.Y);
            // ApplyTransformation(rotate);   
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
            Vector3[] wheelVertices = wheelMesh.vertices; // You may perform additional initialization here if needed
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

        // Get the current rotation matrix of the car
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(transform.rotation);

        // if (translationAmount > 0f && !hasRotatedForward)
        // {
        //     // Calculate the direction to rotate for forward movement
        //     Vector3 moveDirection = transform.forward;

        //     // Calculate the rotation to align the forward direction with the movement direction
        //     Quaternion targetRotation = Quaternion.LookRotation(moveDirection, transform.up);

        //     // Convert the rotation to a matrix
        //     rotationMatrix = Matrix4x4.Rotate(targetRotation);

        //     hasRotatedForward = true; // Set the flag for forward rotation
        //     hasRotatedBackward = false; // Reset the flag for backward rotation
        // }
        // else if (translationAmount < 0f && !hasRotatedBackward)
        // {
        //     // Calculate the direction to rotate for backward movement
        //     Vector3 moveDirection = -transform.forward;

        //     // Calculate the rotation to align the forward direction with the movement direction
        //     Quaternion targetRotation = Quaternion.LookRotation(moveDirection, transform.up);

        //     // Convert the rotation to a matrix
        //     rotationMatrix = Matrix4x4.Rotate(targetRotation);

        //     hasRotatedBackward = true; // Set the flag for backward rotation
        //     hasRotatedForward = false; // Reset the flag for forward rotation
        // }

        // Apply the rotation to the car
        // ApplyTransformation(rotationMatrix);
    }

    bool hasRotated = false; // Keep track of whether rotation has occurred

    void MoveCarLRObject(float translationAmount)
    {
        Matrix4x4 move = HW_TransformsFer.TranslationMat(0f, 0f, translationAmount);
        ApplyTransformation(move);

        // if (translationAmount != 0f && !hasRotated) // Check if the car is moving sideways and hasn't rotated yet
        // {
        //     Vector3 forwardDirection = transform.forward;
        //     Vector3 moveDirection = -transform.right * Mathf.Sign(translationAmount); // Invert transform.right

        //     Quaternion targetRotation = Quaternion.FromToRotation(forwardDirection, moveDirection);
        //     Matrix4x4 rotationMatrix = Matrix4x4.Rotate(targetRotation);

        //     ApplyTransformation(rotationMatrix);

        //     hasRotated = true; // Set the flag to indicate that rotation has been performed
        // }
        // else if (translationAmount == 0f)
        // {
        //     hasRotated = false; // Reset the flag when not moving sideways
        // }

        // // Reset rotation flag if any other movement key is pressed
        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        // {
        //     hasRotated = false;
        // }
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

    
    // void DoTransform(){
    //     //create the matrices
    //     Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x *Time.time , displacement.y *Time.time, displacement.z *Time.time);
    //     Matrix4x4 moveOrigin = HW_Transforms.TranslationMat(-displacement.x, -displacement.y, -displacement.z);
    //     Matrix4x4 moveObject = HW_Transforms.TranslationMat(displacement.x, displacement.y, displacement.z);
    //     Matrix4x4 rotate = HW_Transforms.RotateMat(angle * Time.time, rotationAxis);

    //     //combine the matrices
    //     //operations are executed in backwards order
    //     Matrix4x4 composite =  move * rotate;

    //     // for (int i=0; i<newVertices.Length; i++)
    //     // {
    //     //     Vector4 temp = new Vector4(newVertices[i].x, newVertices[i].y, newVertices[i].z, 1);

    //     //     newVertices[i] = composite * temp;
    //     // }
    //     for (int i=0; i<newVertices.Length; i++)
    //     {
    //         Vector4 temp = new Vector4(baseVertices[i].x, baseVertices[i].y, baseVertices[i].z, 1);

    //         newVertices[i] = composite * temp;
    //     }
        

    //     mesh.vertices = newVertices;
    //     mesh.RecalculateNormals();

    
    // }


    // Function to rotate backward wheels in z axis according to the car movement speed
    void RotateWheels()
    {
        // foreach (GameObject wheelObject in WheelObjects)
        // {
        //     Mesh wheelMesh = wheelObject.GetComponentInChildren<MeshFilter>().mesh;
        //     Vector3[] wheelVertices = wheelMesh.vertices;

        //     // Calculate the center of the wheel in local space
        //     Vector3 wheelCenter = wheelObject.transform.TransformPoint(wheelMesh.bounds.center);

        //     // Rotate around the center of the wheel
        //     Matrix4x4 rotate = HW_TransformsFer.RotateMat(angle * Time.time, AXISFer.Z);

        //     for (int i = 0; i < wheelVertices.Length; i++)
        //     {
        //         Vector4 temp = new Vector4(wheelVertices[i].x, wheelVertices[i].y, wheelVertices[i].z, 1);
        //         wheelVertices[i] = rotate * temp;
        //     }

        //     wheelMesh.vertices = wheelVertices;
        //     wheelMesh.RecalculateNormals();
        // }
    }
}

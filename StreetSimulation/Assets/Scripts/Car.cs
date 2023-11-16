using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 60.0f;
    public float driftSpeedFactor = 0.5f; // Reducción de velocidad durante el derrape
    public float driftRotationFactor = 2.0f; // Aumento de velocidad de rotación durante el derrape

    private bool handBrake = false;

    public Camera leftSideViewCamera;
    public Camera rightSideViewCamera;
    public Camera topViewCamera;

    // Add wheels to the car to make them spin when the car moves with the mesh
    // public WheelCollider[] wheelColliders;
    public Transform[] wheelMeshes;


    public void SwitchToLeftSideView()
    {
        leftSideViewCamera.enabled = true;
        rightSideViewCamera.enabled = false;
        topViewCamera.enabled = false;
    }

    public void SwitchToRightSideView()
    {
        leftSideViewCamera.enabled = false;
        rightSideViewCamera.enabled = true;
        topViewCamera.enabled = false;
    }

    public void SwitchToTopView()
    {
        leftSideViewCamera.enabled = false;
        rightSideViewCamera.enabled = false;
        topViewCamera.enabled = true;
    }

    // Function to move together wheels with the car mesh
    public void MoveWheels()
    {
        foreach (Transform wheelMesh in wheelMeshes)
        {
            wheelMesh.position = transform.position;
        }
    }

    // Function to rotate backward wheels in z axis according to the car movement speed
    public void RotateWheelsBackwards()
    {
        foreach (Transform wheelMesh in wheelMeshes)
        {
            wheelMesh.Rotate(Vector3.forward, speed * Time.deltaTime * 10);
        }
    }
    // Function to rotate forward wheels in z axis according to the car movement speed
    public void RotateWheelsForward()
    {
        foreach (Transform wheelMesh in wheelMeshes)
        {
            wheelMesh.Rotate(Vector3.forward, -speed * Time.deltaTime * 10);
        }
    }




    void Start()
    {
        
    }

    void Update()
    {
        float currentSpeed = handBrake ? speed * driftSpeedFactor : speed;
        float currentRotationSpeed = handBrake ? rotationSpeed * driftRotationFactor : rotationSpeed;

        // Mueve el objeto constantemente en dirección positiva del eje x
        // transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
            // do it only moving the vertices of the mesh not the whole object using matrices

            RotateWheelsForward();
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
            RotateWheelsBackwards();
        }

        // Girar el objeto contra el reloj alrededor del eje y cuando se presiona "a"
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -currentRotationSpeed * Time.deltaTime);
        }

        // Girar el objeto en sentido horario alrededor del eje y cuando se presiona "d"
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);
        }

        // Activar el freno de mano cuando se presiona "F"
        if (Input.GetKeyDown(KeyCode.F))
        {
            handBrake = true;
        }

        // Desactivar el freno de mano cuando se suelta "F"
        if (Input.GetKeyUp(KeyCode.F))
        {
            handBrake = false;
        }

        // Cambiar la cámara a la vista izquierda cuando se presiona "1"
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToLeftSideView();
        }

        // Cambiar la cámara a la vista derecha cuando se presiona "2"
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToRightSideView();
        }

        // Cambiar la cámara a la vista superior cuando se presiona "3"
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToTopView();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelFollow : MonoBehaviour
{
    public Transform carTransform; // Reference to the car's transform

    void Update()
    {
        // Update the wheel position and rotation based on the car's transform
        transform.position = carTransform.position;
        transform.rotation = carTransform.rotation;
    }
}


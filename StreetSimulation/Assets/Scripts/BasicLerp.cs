// /*
// Implement Linear Interpolation between positions (Vector 3)

// David S. Vieyra Garcia
// 2023-11-15
// */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BasicLerp : MonoBehaviour
// {
//     [SerializeField] Vector3 startPosition;
//     [SerializeField] Vector3 endPosition;
//     [Range(0.0f, 1.0f)] [SerializeField] float t; // t
//     [SerializeField] float moveTime = 5f; // moveTime
//     float elapsedTime; // elapsed time

//     // Start is called before the first frame update
//     void Start()
//     {

        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         t = elapsedTime / moveTime;
        
//         t = t * t * (3.0f - 2.0f * t);
        
//         Vector3 position = startPosition + (endPosition - startPosition) * t;

//         // Move the object using the Unity transforms
//         transform.position = position;
//         Matrix4x4 move = HW_Transforms.TranslationMat(position.x, position.y, position.z);

//         elapsedTime += Time.deltaTime;

//         if (elapsedTime > moveTime)
//         {
//             Vector3 temp = startPosition;
//             elapsedTime = 0.0f;
//             startPosition = endPosition;
//             endPosition = temp;
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLerp : MonoBehaviour
{
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 endPosition;
    [Range(0.0f, 1.0f)] [SerializeField] float t; // t
    [SerializeField] float moveTime = 5f; // moveTime
    float elapsedTime; // elapsed time

    [SerializeField] GameObject[] wheels; // Array to hold the wheel objects
    Vector3[] wheelInitialLocalPositions; // Array to hold initial local positions of wheels

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the wheel objects array
       
        // Store initial local positions of wheels relative to the car
        wheelInitialLocalPositions = new Vector3[wheels.Length];
        for (int i = 0; i < wheels.Length; i++)
        {
            wheelInitialLocalPositions[i] = wheels[i].transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        t = elapsedTime / moveTime;

        t = t * t * (3.0f - 2.0f * t);

        Vector3 position = startPosition + (endPosition - startPosition) * t;

        // Move the object using the Unity transforms
        transform.position = position;
        Matrix4x4 move = HW_Transforms.TranslationMat(position.x, position.y, position.z);

        elapsedTime += Time.deltaTime;

        if (elapsedTime > moveTime)
        {
            Vector3 temp = startPosition;
            elapsedTime = 0.0f;
            startPosition = endPosition;
            endPosition = temp;
        }

        // Update the position of the wheels based on the car's position and initial local positions
        for (int i = 0; i < wheels.Length; i++)
        {
            // Calculate the transformed position based on the car's movement and the initial local position of the wheel

            // Update the wheel's position
            wheels[i].transform.position = position + move.MultiplyPoint3x4(wheelInitialLocalPositions[i]);
            Matrix4x4 moveWheels = HW_Transforms.TranslationMat(position.x, position.y, position.z);
        }
    }
}

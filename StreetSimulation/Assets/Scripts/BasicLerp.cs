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



// Ya casi cuando se mueve hacia delante rota hacia la derecha y cuando se mueve hacia atras rota hacia la izquierda
// /*
// Implement Linear Interpolation between positions (Vector 3) with immediate rotation towards the direction of movement

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

//         // Calculate direction of movement
//         Vector3 direction = (endPosition - startPosition).normalized;

//         // Rotate towards the direction of movement (immediate rotation)
//         if (direction != Vector3.zero)
//         {
//             Quaternion targetRotation = Quaternion.LookRotation(direction);
//             transform.rotation = targetRotation;
//         }

//         // Move the object using the Unity transforms
//         transform.position = position;

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

// Ya funciona pero sin llantas.
// /*
// Implement Linear Interpolation between positions (Vector 3) with corrected rotation towards the direction of movement

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

//         // Calculate direction of movement
//         Vector3 direction = (endPosition - startPosition).normalized;

//         // Rotate towards the direction of movement with a 90-degree offset
//         if (direction != Vector3.zero)
//         {
//             Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
//             transform.rotation = targetRotation;
//         }

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

// /*
// Implement Linear Interpolation between positions (Vector 3) with corrected rotation towards the direction of movement,
// and have four child objects follow the same movement and rotation

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

//     [SerializeField] List<Transform> followerObjects; // List of objects to follow

//     // Start is called before the first frame update
//     void Start()
//     {
//         // Initialize followerObjects list if not assigned
//         if (followerObjects == null)
//             followerObjects = new List<Transform>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         t = elapsedTime / moveTime;
//         t = t * t * (3.0f - 2.0f * t);
        
//         Vector3 position = startPosition + (endPosition - startPosition) * t;

//         // Calculate direction of movement
//         Vector3 direction = (endPosition - startPosition).normalized;

//         // Rotate towards the direction of movement with a 90-degree offset
//         if (direction != Vector3.zero)
//         {
//             Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
//             transform.rotation = targetRotation;
//         }

//         // Move the object using the Unity transforms
//         transform.position = position;

//         // Update follower objects
//         foreach (Transform follower in followerObjects)
//         {
//             follower.position = position;
//             follower.rotation = transform.rotation;
//         }

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

//     [SerializeField] List<Transform> followerObjects; // List of objects to follow
//     private List<Vector3> initialOffsets; // Initial offsets between main object and followers

//     [SerializeField] float wheelRotationSpeed = 1f; // Speed of wheel rotation (degrees per second)

//     // Start is called before the first frame update
//     void Start()
//     {
//         // Initialize followerObjects list if not assigned
//         if (followerObjects == null)
//             followerObjects = new List<Transform>();

//         // Initialize initialOffsets list
//         initialOffsets = new List<Vector3>();

//         // Calculate initial offset between main object and each follower
//         foreach (Transform follower in followerObjects)
//         {
//             Vector3 initialOffset = transform.InverseTransformPoint(follower.position);
//             initialOffsets.Add(initialOffset);
//         }
//     }

// // Update is called once per frame
//     void Update()
//     {
//         t = elapsedTime / moveTime;
//         t = t * t * (3.0f - 2.0f * t);
        
//         Vector3 position = startPosition + (endPosition - startPosition) * t;

//         // Calculate direction of movement
//         Vector3 direction = (endPosition - startPosition).normalized;

//         // Rotate towards the direction of movement with a 90-degree offset
//         if (direction != Vector3.zero)
//         {
//             Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
//             transform.rotation = targetRotation;
//         }

//         // Move the object using the Unity transforms
//         transform.position = position;

//         // Update follower objects with individual initial offsets
//         for (int i = 0; i < followerObjects.Count; i++)
//         {
//             followerObjects[i].position = transform.TransformPoint(initialOffsets[i]);
//             followerObjects[i].rotation = transform.rotation;

//             // Calculate rotation angle based on the car's movement
//             float rotationAngle = wheelRotationSpeed * Time.deltaTime;
//             followerObjects[i].Rotate(Vector3.forward, rotationAngle, Space.Self);
//         }

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



// /*
// The wheel rotation now works.
// Implement Linear Interpolation between positions (Vector 3) with corrected rotation towards the direction of movement,
// and have four child objects maintain individual relative positions, rotation, and rotate as wheels while following the main object's movement and rotation

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

//     [SerializeField] List<Transform> followerObjects; // List of objects to follow
//     private List<Vector3> initialOffsets; // Initial offsets between main object and followers

//     [SerializeField] float wheelRotationSpeed = 360f; // Wheel rotation speed in degrees per second
//     float wheelRotationTime = 0f; // Elapsed time for wheel rotation

//     // Start is called before the first frame update
//     void Start()
//     {
//         // Initialize followerObjects list if not assigned
//         if (followerObjects == null)
//             followerObjects = new List<Transform>();

//         // Initialize initialOffsets list
//         initialOffsets = new List<Vector3>();

//         // Calculate initial offset between main object and each follower
//         foreach (Transform follower in followerObjects)
//         {
//             Vector3 initialOffset = transform.InverseTransformPoint(follower.position);
//             initialOffsets.Add(initialOffset);
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         t = elapsedTime / moveTime;
//         t = t * t * (3.0f - 2.0f * t);
        
//         Vector3 position = startPosition + (endPosition - startPosition) * t;

//         // Calculate direction of movement
//         Vector3 direction = (endPosition - startPosition).normalized;

//         // Rotate towards the direction of movement with a 90-degree offset
//         if (direction != Vector3.zero)
//         {
//             Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
//             transform.rotation = targetRotation;
//         }

//         // Move the object using the Unity transforms
//         transform.position = position;

//         // Update follower objects with individual initial offsets and rotate wheels
//         for (int i = 0; i < followerObjects.Count; i++)
//         {
//             followerObjects[i].position = transform.TransformPoint(initialOffsets[i]);
//             followerObjects[i].rotation = transform.rotation;

//             // Rotate wheels
//             RotateWheel(followerObjects[i]);
//         }

//         elapsedTime += Time.deltaTime;

//         if (elapsedTime > moveTime)
//         {
//             Vector3 temp = startPosition;
//             elapsedTime = 0.0f;
//             startPosition = endPosition;
//             endPosition = temp;
//         }
//     }

//     // Rotate the wheel object around its local z-axis
//     void RotateWheel(Transform wheel)
//     {
//         wheelRotationTime += Time.deltaTime;
//         // wheel.localRotation = Quaternion.Euler(0f, 0f, wheelRotationSpeed * wheelRotationTime);
//         wheel.Rotate(Vector3.forward, wheelRotationSpeed * wheelRotationTime);
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

    [SerializeField] List<Transform> followerObjects; // List of objects to follow
    private List<Vector3> initialOffsets; // Initial offsets between main object and followers

    [SerializeField] float maxSpeed = 10f; // Maximum speed of the main object
    [SerializeField] float wheelRotationSpeedMultiplier = 50f; // Multiplier for wheel rotation speed

    float wheelRotationTime = 0f; // Elapsed time for wheel rotation

    Vector3 QuaternionToEuler(Quaternion rotation)
    {
        return rotation.eulerAngles;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Initialize followerObjects list if not assigned
        if (followerObjects == null)
            followerObjects = new List<Transform>();

        // Initialize initialOffsets list
        initialOffsets = new List<Vector3>();

        // Calculate initial offset between main object and each follower
        foreach (Transform follower in followerObjects)
        {
            Vector3 initialOffset = transform.InverseTransformPoint(follower.position);
            initialOffsets.Add(initialOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        t = elapsedTime / moveTime;
        t = t * t * (3.0f - 2.0f * t);

        Vector3 position = startPosition + (endPosition - startPosition) * t;

        // Calculate direction and speed of movement
        Vector3 direction = (endPosition - startPosition).normalized;
        float speed = (endPosition - startPosition).magnitude / moveTime;

        // Rotate towards the direction of movement with a 90-degree offset
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
            transform.rotation = targetRotation;
            Matrix4x4 rotate = HW_TransformsFer.RotateMat(targetRotation.eulerAngles.y, Vector3.up);
            
        }

        // Move the object using the Unity transforms
        transform.position = position;
        Matrix4x4 move = HW_Transforms.TranslationMat(position.x, position.y, position.z);

        // Update follower objects with individual initial offsets and rotate wheels
        for (int i = 0; i < followerObjects.Count; i++)
        {
            Vector3 followerPosition = transform.TransformPoint(initialOffsets[i]); // Get follower's position in world space

            // Set follower's position using a Matrix4x4 translation
            Matrix4x4 moveFollower = HW_Transforms.TranslationMat(position.x, position.y, position.z);
            
            // Set follower's rotation (similar to the main object's rotation)
            Quaternion followerTargetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0); // Adjust the rotation as needed
            Matrix4x4 rotateFollower = HW_TransformsFer.RotateMat(followerTargetRotation.eulerAngles.y, Vector3.up);

            // Apply position and rotation transformations to the follower object
            followerObjects[i].position = followerPosition; // Update the position directly
            followerObjects[i].rotation = followerTargetRotation; // Update the rotation directly

            // Store the move and rotate matrices for further use
            // Matrix4x4 moveFollower = ...; // Store move matrix for this follower
            // Matrix4x4 moveFollwer = hola;
            // Matrix4x4 rotateFollower = ...; // Store rotate matrix for this follower

            // Rotate wheels based on the speed of the main object
            RotateWheel(followerObjects[i], speed);
        }


        elapsedTime += Time.deltaTime;

        if (elapsedTime > moveTime)
        {
            Vector3 temp = startPosition;
            elapsedTime = 0.0f;
            startPosition = endPosition;
            endPosition = temp;
        }
    }

    // // Rotate the wheel object around its local z-axis
    // void RotateWheel(Transform wheel, float speed)
    // {
    //     wheelRotationTime += Time.deltaTime;
    //     float rotationAmount = -speed * wheelRotationSpeedMultiplier * wheelRotationTime;
    //     wheel.Rotate(Vector3.forward, rotationAmount);

    // }

    // Rotate the wheel object around its local z-axis
    void RotateWheel(Transform wheel, float speed)
    {
        wheelRotationTime += Time.deltaTime;
        float rotationAmount = -speed * wheelRotationSpeedMultiplier * wheelRotationTime;
        Matrix4x4 rotateWheel = HW_TransformsFer.RotateMat(rotationAmount, wheel.forward);
        wheel.Rotate(Vector3.forward, rotationAmount);

        // Apply additional rotations using Matrix4x4 if needed
        // Get the local position of the wheel in world space
        // Vector3 wheelWorldPos = wheel.TransformPoint(Vector3.zero);

        // Calculate rotation matrix for the wheel around its local z-axis
        

        // Apply rotation to the wheel's position using matrix multiplication
        // wheel.position = rotateWheel.MultiplyPoint3x4(wheelWorldPos);
    }

}
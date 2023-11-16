/*
Implement Linear Interpolation between positions (Vector 3)

David S. Vieyra Garcia
2023-11-15
*/

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

    // Start is called before the first frame update
    void Start()
    {

        
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
    }
}

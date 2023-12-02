// using UnityEngine;

// public class TrafficLightController : MonoBehaviour
// {
//     [SerializeField] public bool isGreen = false;
//     public GameObject greenLight;
//     public GameObject redLight;

//     public float lightChangeInterval = 5.0f; // Time interval for light change in seconds
//     private float timer;

//     void Start()
//     {
//         // Ensure the initial state of lights
//         ToggleLights();
//         timer = lightChangeInterval; // Start the timer
//     }

//     void Update()
//     {
//         ToggleLights();
//     }

//     void ToggleLights()
//     {
//         // Activate/deactivate lights based on the boolean value
//         if (isGreen)
//         {
//             greenLight.SetActive(true);
//             redLight.SetActive(false);
//         }
//         else
//         {
//             greenLight.SetActive(false);
//             redLight.SetActive(true);
//         }
//     }
// }

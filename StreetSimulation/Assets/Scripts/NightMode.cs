using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMode : MonoBehaviour
{
    public Light directionalLight;
    public Color nightColor = new Color(0.1f, 0.1f, 0.2f); // Adjust the color as needed
    public float nightIntensity = 0.2f; // Adjust the intensity as needed

    void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = GameObject.FindWithTag("Sun").GetComponent<Light>();
        }

        if (directionalLight != null)
        {
            ApplyNightMode();
        }
        else
        {
            Debug.LogError("Directional light not found. Make sure it's tagged or assign it manually.");
        }
    }

    void ApplyNightMode()
    {
        directionalLight.color = nightColor;
        directionalLight.intensity = nightIntensity;
    }
}

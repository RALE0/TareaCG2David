using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    Camera m_MainCamera;
    public Camera m_LeftCamera;
    public Camera m_RightCamera;
    public Camera m_FrontCamera;
    public Camera m_BackCamera;

    public void Start()
    {
        m_MainCamera = Camera.main;
        m_MainCamera.enabled = true;
        m_LeftCamera.enabled = false;
        m_RightCamera.enabled = false;
        m_FrontCamera.enabled = false;
        m_BackCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Switching to main camera");
            m_MainCamera.enabled = true;
            m_LeftCamera.enabled = false;
            m_RightCamera.enabled = false;
            m_FrontCamera.enabled = false;
            m_BackCamera.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Switching to left camera");
            m_LeftCamera.enabled = true;
            m_MainCamera.enabled = false;
            m_RightCamera.enabled = false;
            m_FrontCamera.enabled = false;
            m_BackCamera.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Switching to right camera");
            m_RightCamera.enabled = true;
            m_MainCamera.enabled = false;
            m_LeftCamera.enabled = false;
            m_FrontCamera.enabled = false;
            m_BackCamera.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Switching to up camera");
            m_FrontCamera.enabled = true;
            m_MainCamera.enabled = false;
            m_RightCamera.enabled = false;
            m_LeftCamera.enabled = false;
            m_BackCamera.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Switching to down camera");
            m_MainCamera.enabled = false;
            m_RightCamera.enabled = false;
            m_FrontCamera.enabled = false;
            m_LeftCamera.enabled = false;
            m_BackCamera.enabled = true;
        }
    }
}


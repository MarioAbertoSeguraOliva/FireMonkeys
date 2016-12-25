using UnityEngine;
using System.Collections;

public class ActivateCamera : MonoBehaviour {

    public new GameObject camera;
    public Camera playerCamera;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DisableAllCameras();
            camera.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            camera.SetActive(false);
            playerCamera.enabled = true;
        }
    }

    void DisableAllCameras()
    {
        foreach (Camera c in Camera.allCameras) c.enabled = false;
    }
}

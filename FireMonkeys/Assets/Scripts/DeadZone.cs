using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour {

    public int sceneIndex = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") SceneManager.LoadScene(sceneIndex);
    }
}

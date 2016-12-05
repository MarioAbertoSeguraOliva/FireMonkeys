using UnityEngine;
using System.Collections;

public class PauseGameScript : MonoBehaviour {

    bool paused = false;
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
            paused = true;
        if (Input.GetKeyDown(KeyCode.Escape) && paused)
            paused = false;
        Debug.Log("Paused =" + paused);
	}
}

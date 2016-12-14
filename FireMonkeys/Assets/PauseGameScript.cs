using UnityEngine;
using System.Collections;

public class PauseGameScript : MonoBehaviour {
    public GameObject ga;

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
	}

    public void PauseGame()
    {
        ga.SetActive(!ga.activeSelf);
        Time.timeScale = ga.activeSelf ? 0.0f : 1.0f;
    }
}

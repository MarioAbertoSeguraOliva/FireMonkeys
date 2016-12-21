using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class PauseGameScript : MonoBehaviour {
    public GameObject ga;
    public Animation anim;

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
	}

    public void PauseGame()
    {
        StartCoroutine(AnimOnPaused(!ga.activeSelf));
        if (!ga.activeSelf) TogglePause(true);
    }

    private void TogglePause(bool startPause)
    {
        ga.SetActive(startPause);
        Time.timeScale = startPause ? 0.0f : 1.0f;
        Camera.main.GetComponent<BlurOptimized>().enabled = startPause;
    }

    IEnumerator AnimOnPaused(bool startPause)
    {
        
        AnimationState state = anim[anim.clip.name];
        float startTime = Time.realtimeSinceStartup;
        while (anim.clip.length > Time.realtimeSinceStartup - startTime)
        {
            anim.Play(anim.clip.name);

            if(startPause)
                state.time =  Time.realtimeSinceStartup - startTime;
            else
                state.time = anim.clip.length - (Time.realtimeSinceStartup - startTime);

            anim.Sample();

            anim.Stop();

            yield return 0;
        }
        if (!startPause) TogglePause(false);
    }
}

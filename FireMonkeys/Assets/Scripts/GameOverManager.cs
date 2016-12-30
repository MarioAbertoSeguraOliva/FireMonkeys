using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class GameOverManager : MonoBehaviour {
    public GameObject canvasGameObject;
    public GameObject cameraToPutEffect;
    public Vector3 respawnPosition;
    public GameObject player;
    public GameObject heartContainers;

    public void OnGameOver()
    {
        for (int i = 0; i < heartContainers.transform.childCount; i++)
            heartContainers.transform.GetChild(i).gameObject.SetActive(true);

        canvasGameObject.SetActive(true);
        canvasGameObject.GetComponent<AudioSource>().Play();
        canvasGameObject.GetComponent<Animation>().wrapMode = WrapMode.Once;
        canvasGameObject.GetComponent<Animation>().Play();
        cameraToPutEffect.GetComponent<BlurOptimized>().enabled = true;

        StartCoroutine(CheckClick());
    }

    IEnumerator CheckClick()
    {
        yield return new WaitForSeconds(2);
        while (!Input.GetMouseButtonDown(0)) 
            yield return new WaitForFixedUpdate();

        canvasGameObject.GetComponent<AudioSource>().Stop();

        canvasGameObject.SetActive(false);
        cameraToPutEffect.GetComponent<BlurOptimized>().enabled = false;
        player.transform.position = respawnPosition;
        player.GetComponent<ClimbCharacterUserControl>().Respawn();
        
    }

}

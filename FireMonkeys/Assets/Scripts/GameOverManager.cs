using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class GameOverManager : MonoBehaviour {
    public GameObject canvasGameObject;
    public GameObject cameraToPutEffect;
    public Transform respawnPosition;
    public GameObject player;

    public void OnGameOver()
    {
        canvasGameObject.SetActive(true);
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
        
        

        canvasGameObject.SetActive(false);
        cameraToPutEffect.GetComponent<BlurOptimized>().enabled = false;
        player.transform.position = respawnPosition.position;
        player.GetComponent<ClimbCharacterUserControl>().Respawn();
        
    }

}

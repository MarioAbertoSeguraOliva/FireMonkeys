using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    //public int SceneIndexToLoad;
    public string sceneNameToLoad;
    public GameObject ObjectToMove;
    public GameObject loadingImage;

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.MoveGameObjectToScene(ObjectToMove, SceneManager.GetSceneByName(sceneNameToLoad));
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    public void LoadScene(string SceneIndexToLoad)
    {
        loadingImage.SetActive(true);
        SceneManager.LoadScene(sceneNameToLoad);
    }
}

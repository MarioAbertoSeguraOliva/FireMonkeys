using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public int SceneIndexToLoad;
    public GameObject ObjectToMove;
    public GameObject loadingImage;

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.MoveGameObjectToScene(ObjectToMove, SceneManager.GetSceneAt(SceneIndexToLoad));
            SceneManager.LoadScene(SceneIndexToLoad);
        }
    }

    public void LoadScene(int SceneIndexToLoad)
    {
        loadingImage.SetActive(true);
        SceneManager.LoadScene(SceneIndexToLoad);
    }
}

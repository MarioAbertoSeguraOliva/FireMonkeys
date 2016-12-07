using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public int SceneIndexToLoad;
    public GameObject ObjectToMove;

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.MoveGameObjectToScene(ObjectToMove, SceneManager.GetSceneAt(SceneIndexToLoad));
            SceneManager.LoadScene(SceneIndexToLoad);
        }
    }
}

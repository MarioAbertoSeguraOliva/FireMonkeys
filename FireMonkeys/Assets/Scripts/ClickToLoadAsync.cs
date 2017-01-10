using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickToLoadAsync : MonoBehaviour
{

    public Slider loadingBar;
    public GameObject loadingImage;
    private AsyncOperation async;

    public void ClickAsync(string level)
    {
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }


    IEnumerator LoadLevelWithBar(string sceneNameToLoad)
    {
        async = SceneManager.LoadSceneAsync(sceneNameToLoad);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }
}
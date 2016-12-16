using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickToLoadAsync : MonoBehaviour
{

    public Slider loadingBar;
    public GameObject loadingImage;
    private AsyncOperation async;

    public void ClickAsync(int level)
    {
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }


    IEnumerator LoadLevelWithBar(int level)
    {
        async = SceneManager.LoadSceneAsync(level);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }
}
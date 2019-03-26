using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour {

    public Image SceneFadeOverlay;
    public Slider ProgressSlider;
    public Image LoadingSceneFadeOverlay;

    private AsyncOperation _loadingAsyncOperation;

    public float WaitOnLoadEnd = 1f;
    public float FadeDuration = 1f;

    public static string LoadingSceneName = "LoadingScene";
    public static string SceneToLoad;


    void Awake()
    {
        SceneFadeOverlay.gameObject.SetActive(true);
        ProgressSlider.gameObject.SetActive(false);
        LoadingSceneFadeOverlay.gameObject.SetActive(false);

        StartCoroutine(LoadSceneAsync(SceneToLoad));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return StartCoroutine(Fade(SceneFadeOverlay, FadeDuration, true));      //Fade into black, unload old scene
        //execution continues after Fade is over
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        LoadingSceneFadeOverlay.gameObject.SetActive(true);       //objects are inactive at start so set them to active
        ProgressSlider.gameObject.SetActive(true);                
        yield return StartCoroutine(Fade(LoadingSceneFadeOverlay, FadeDuration, false)); //fade out the overlay to the slider is visible


        _loadingAsyncOperation = SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);  //Load the chosen scene in the background while displaying progress on the slider
        _loadingAsyncOperation.allowSceneActivation = false;                   //prevent it from activating immediately, wait until the slider is full and another fade is performed         
        float loadingProgress = 0.0f;
        // operation does not auto-activate scene, so it's stuck at 0.9 instead of 1
        while (_loadingAsyncOperation.progress < 0.9f)
        {
            yield return null;

            if(Mathf.Approximately(loadingProgress, _loadingAsyncOperation.progress) == false)
            {
                loadingProgress = _loadingAsyncOperation.progress;
                ProgressSlider.value = loadingProgress;
            }
        }
        ProgressSlider.value = 1.0f;
        yield return new WaitForSeconds(WaitOnLoadEnd);
        yield return StartCoroutine(Fade(LoadingSceneFadeOverlay, FadeDuration, true)); //fade into black once loading is complete and the slider is full

        _loadingAsyncOperation.allowSceneActivation = true;      //allow loading completion & disable slider and overlay
        ProgressSlider.gameObject.SetActive(false);
        LoadingSceneFadeOverlay.gameObject.SetActive(false);

        yield return StartCoroutine(Fade(SceneFadeOverlay, FadeDuration * 0.5f, false)); //the other overlay is still at full opacity so fade out from it into the new scene

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(LoadingSceneName)); //apparenty Async loading/unloading doesn't work unless there's a coroutine going 
    }                                                                                 //so this can only be called after a coroutine

    IEnumerator Fade(Image image, float fadeTime, bool fadeType) 
    {
        float startOpacity = fadeType ? 0.0f : 1.0f; //depending on fadeType either fades in or fades out
        float endOpacity = fadeType ? 1.0f : 0.0f;

        Color color = image.color;
        color.a = startOpacity;

        float stopwatch = 0.0f;

        while(stopwatch <= fadeTime)
        {
            stopwatch += Time.deltaTime;

            color.a = Mathf.Lerp(startOpacity, endOpacity, stopwatch / fadeTime);
            image.color = color;

            yield return null;
        }

        color.a = endOpacity;
        image.color = color;
    }

    public static void LoadScene(string sceneName)
    {
        SceneToLoad = sceneName;
        SceneManager.LoadScene(LoadingSceneName, LoadSceneMode.Additive);
    }


}

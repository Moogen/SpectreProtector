using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchScenes : MonoBehaviour
{
    new GameObject camera;
    FadeOut fo;
    [SerializeField] private PersistentString lastOpenedScene;

    #region Singleton Pattern
    private static LaunchScenes _instance;
    public static LaunchScenes Instance
    {
        get
        {
            return _instance;
        }

    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        camera = GameObject.Find("Main Camera");
        fo = camera.GetComponent<FadeOut>();
    }

    public void LaunchScene(int sceneBuildIndex)
    {
        StartCoroutine(LoadSceneAsynchronous(sceneBuildIndex));
    }

    public void LaunchScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsynchronous(sceneName));
    }

    public void LaunchSceneAndStorePrevious(int sceneBuildIndex)
    {
        lastOpenedScene.text = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAsynchronous(sceneBuildIndex));
    }

    public void LaunchSceneAndStorePrevious(string sceneName)
    {
        lastOpenedScene.text = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAsynchronous(sceneName));
    }

    public void LaunchPreviousScene()
    {
        StartCoroutine(LoadSceneAsynchronous(lastOpenedScene.text));
    }

    private IEnumerator LoadSceneAsynchronous(int sceneBuildIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneBuildIndex);
        while (!op.isDone)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator LoadSceneAsynchronous(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void Quit()
    {
        lastOpenedScene.text = "MainMenu";
        Application.Quit();
    }

    public void FadeOut()
    {
        fo.enabled = true;
        StartCoroutine(WaitASecond());
    }

    private IEnumerator WaitASecond()
    {
        float t = 0.0f; 
        while (t < 1)
        {
            t += Time.deltaTime;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
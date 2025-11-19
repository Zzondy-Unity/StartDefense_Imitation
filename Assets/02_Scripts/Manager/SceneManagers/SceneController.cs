using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, IManager
{
    public SceneName priviousScene { get; private set; }
    public SceneName curScene { get; private set; }
    
    public BaseSceneManager curSceneManager { get; private set; }
    
    private Coroutine loadingCoroutine;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        curSceneManager = FindObjectOfType<BaseSceneManager>();
        if (curSceneManager != null)
        {
            curSceneManager.Init();
        }
    }
    
    // 씬 전환 기능
    public void LoadScene(SceneName scene)
    {
        if (loadingCoroutine != null)
        {
            Logger.WarningLog($"[SceneController] {scene} can't be loaded because it is already loading.");
            return;
        }
        
        OnExit();
        priviousScene = curScene;
        curScene = scene;

        loadingCoroutine = StartCoroutine(LoadSceneAsync(scene.ToString()));
    }

    private IEnumerator LoadSceneAsync(string SceneName)
    {
        var operation = SceneManager.LoadSceneAsync(SceneName);

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        loadingCoroutine = null;
        OnEnter();
    }

    private void OnExit()
    {
        curSceneManager?.OnExit();
    }

    private void OnEnter()
    {
        curSceneManager?.OnEnter();
    }

    public void Init()
    {
        
    }
}

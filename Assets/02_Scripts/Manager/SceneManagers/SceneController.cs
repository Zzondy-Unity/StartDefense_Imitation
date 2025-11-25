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
    
    public void Init()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        curSceneManager = FindObjectOfType<BaseSceneManager>();
        if (curSceneManager != null)
        {
            curSceneManager.Init();
        }
        OnEnter();
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
        
    }

    private void OnExit()
    {
        curSceneManager?.OnExit();
    }

    // 씬 로딩 전에 호출됨
    private void OnEnter()
    {
        Logger.Log($"[SceneController] Entering scene. Scene:{curScene}, curSceneManager:{curSceneManager}");
        curSceneManager?.OnEnter();
    }

    public T GetSceneManager<T>() where T : BaseSceneManager
    {
        return (T)curSceneManager;
    }
}

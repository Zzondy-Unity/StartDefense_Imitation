using UnityEngine;

/// <summary>
/// 매니저 총괄 클래스이며 싱글톤입니다.
/// </summary>
public class GameManager : SingletonDontDestroy<GameManager>
{
    private SceneController _sceneController;

    private ResourceManager _resourceManager;
    private SoundManager _soundManager;
    private DataManager _dataManager;
    private UIManager _uiManager;

    public static ResourceManager Resource
    {
        get { return Instance._resourceManager; }
    }

    public static SoundManager Sound
    {
        get { return Instance._soundManager; }
    }

    public static SceneController Scene
    {
        get { return Instance._sceneController; }
    }

    public static DataManager Data
    {
        get { return Instance._dataManager; }
    }

    public static ObjectPoolManager Pool
    {
        get { return Instance._sceneController.curSceneManager?.objectPoolManager; }
    }

    public static UIManager UI
    {
        get { return Instance._uiManager; }
    }

    protected override void Awake()
    {
        base.Awake();
        if (_instance != this)
        {
            return;
        }

        InitializeManagers();
    }

    /// <summary>
    /// 매니저를 생성 및 초기화합니다.
    /// MonoBehaviour인 매니저는 CreateManager를하고 순서에 맞게 초기화합니다.
    /// </summary>
    private void InitializeManagers()
    {
        _resourceManager = new ResourceManager();
        _dataManager = new DataManager();
        _soundManager = CreateManager<SoundManager>(Instance.transform);
        _sceneController = CreateManager<SceneController>(Instance.transform);
        _uiManager = CreateManager<UIManager>(Instance.transform);

        _sceneController.Init();
        _resourceManager.Init();
        _dataManager.Init();
        _uiManager.Init();
        _soundManager.Init();
    }

    private T CreateManager<T>(Transform parent) where T : Component, IManager
    {
        T manager = GetComponentInChildren<T>(parent);
        if (manager == null)
        {
            GameObject obj = new GameObject(typeof(T).Name);
            manager = obj.AddComponent<T>();
            obj.transform.SetParent(parent);
        }

        return manager;
    }
}
using UnityEngine;

/// <summary>
/// 매니저 총괄 클래스이며 싱글톤입니다.
/// </summary>
public class Managers : SingletonDontDestroy<Managers>
{
    
    private ResourceManager _resourceManager;
    private GameManager _gameManager;
    private TileManager _tileManager;
    private SoundManager _soundManager;
    
    public static ResourceManager Resource { get { return Instance._resourceManager; } }
    public static GameManager Game { get { return Instance._gameManager; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static TileManager Tile { get { return Instance._tileManager; } }

    protected override void Awake()
    {
        base.Awake();
        
        InitializeManagers();
    }
    
    /// <summary>
    /// 매니저를 생성 및 초기화합니다.
    /// MonoBehaviour인 매니저는 CreateManager를하고 순서에 맞게 초기화합니다.
    /// </summary>
    private void InitializeManagers()
    {
        _gameManager = new GameManager();
        _resourceManager = new ResourceManager();
        _soundManager = CreateManager<SoundManager>(Instance.transform);
        _tileManager = CreateManager<TileManager>(Instance.transform);

        _resourceManager.Init();
        _gameManager.Init();
        _soundManager.Init();
        _tileManager.Init();
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

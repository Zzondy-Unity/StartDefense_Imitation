using UnityEngine;

public abstract class BaseSceneManager : Singleton<BaseSceneManager>, IManager
{
    public abstract SceneName curScene { get;}
    public ObjectPoolManager objectPoolManager { get; protected set; }

    /// <summary>
    /// 각 씬 매니저 초기화
    /// </summary>
    public virtual void Init()
    {
        objectPoolManager = GetComponent<ObjectPoolManager>();
    }
    /// <summary>
    /// 입장할때마다 해야할 일
    /// 씬 로딩이 완료된 후 호출됩니다.
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// 씬 로딩이 시작하기 전에 호출됩니다.
    /// </summary>
    public abstract void OnExit();
}

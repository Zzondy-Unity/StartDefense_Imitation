using System;
using UnityEngine;

public class SingletonDontDestroy<T> :  Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}

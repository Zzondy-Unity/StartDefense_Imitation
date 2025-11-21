using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.datas;
    }
    
    [Serializable]
    private class Wrapper<T>
    {
        public List<T> datas;
    }
}
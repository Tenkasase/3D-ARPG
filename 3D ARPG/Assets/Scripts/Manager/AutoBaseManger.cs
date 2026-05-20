using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动挂载式 单例模式基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class AutoBaseManger<T> : MonoBehaviour where T : MonoBehaviour
{    
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
}

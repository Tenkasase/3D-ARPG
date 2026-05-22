using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class BaseManager<T> where T : class
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type type = typeof(T);
                ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                                          null,
                                                                           new Type[0],
                                                                        null);
                if(constructorInfo == null)
                    Debug.LogError("No private constructor found for " + type.Name);
                instance = constructorInfo.Invoke(null) as T;
            }
            return instance;
        }
    }
}
 
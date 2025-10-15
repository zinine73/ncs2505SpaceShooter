using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static bool suttingDown = false;
    static object _lock = new object();
    private static T instance;
    public static T Instance
    {
        get
        {
            if (suttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (instance == null)
                    {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString()
                            + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return instance;
            }
        }
    }
    void OnApplicationQuit()
    {
        suttingDown = true;
    }
    void OnDestroy()
    {
        suttingDown = true;
    }
}

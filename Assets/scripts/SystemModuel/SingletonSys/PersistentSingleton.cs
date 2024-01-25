using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }
    void Awake()
    {
        if (instance == null)
            instance = this as T;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var objs = FindObjectOfType(typeof(T)) as T[];
                if (objs != null && objs.Length > 0)
                    instance = objs[0];

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}

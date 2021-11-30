using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public static CharacterControl instance;

    public bool _isActive;
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        // character active or inactive
        if (_isActive)
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;      
        else
            transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;     
    }
}

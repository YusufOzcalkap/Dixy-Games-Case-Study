using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    //Count Character
    void Update()
    {
        if (EnemyAI.instance._characterCount > 0 && GameManager.instance.gamestate == GameManager.GameState.InGame)
        {
            for (int i = 0; i < EnemyAI.instance._characterCount; i++)
            {
                transform.GetChild(i).GetComponent<CharacterControl>()._isActive = true;
            }
            for (int i = EnemyAI.instance._characterCount; i < 42; i++)
            {
                transform.GetChild(i).GetComponent<CharacterControl>()._isActive = false;
            }
        }
    }
}

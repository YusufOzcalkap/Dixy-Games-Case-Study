using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    void Update()
    {
        // Count Character
        if (BallController.instance._characterCount > 0 && GameManager.instance.gamestate == GameManager.GameState.InGame)
        {
            for (int i = 0; i < BallController.instance._characterCount; i++)
            {
                transform.GetChild(i).GetComponent<CharacterControl>()._isActive = true;          
            }
            for (int i = BallController.instance._characterCount; i < 42; i++)
            {
                transform.GetChild(i).GetComponent<CharacterControl>()._isActive = false;
            }
        }
    }
}

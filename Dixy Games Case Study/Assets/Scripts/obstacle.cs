using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    public enum Obstacle
    {
        Obstacle1,
        Obstacle2,
        Obstacle3
    }
    public Obstacle _obstacle;

    private float _obstacleSpeed;
    void Start()
    {
        _obstacleSpeed = 0.15f;
    }

    void Update()
    {
        if (_obstacle == Obstacle.Obstacle1)
            transform.GetChild(0).eulerAngles = new Vector3(transform.GetChild(0).eulerAngles.x, transform.GetChild(0).eulerAngles.y + 2, transform.GetChild(0).eulerAngles.z);

        if (_obstacle == Obstacle.Obstacle2)
        {
            if (transform.position.x >= 4)
                _obstacleSpeed = 0.15f;

            else if (transform.position.x <= -4)
                _obstacleSpeed = -0.15f;
            
            transform.Translate(Vector3.left * _obstacleSpeed);
        }

        if (_obstacle == Obstacle.Obstacle3)
        {
            if (transform.position.x > 0)
                transform.GetChild(0).eulerAngles = new Vector3(transform.GetChild(0).eulerAngles.x, transform.GetChild(0).eulerAngles.y - 2, transform.GetChild(0).eulerAngles.z);

            if (transform.position.x < 0)
                transform.GetChild(0).eulerAngles = new Vector3(transform.GetChild(0).eulerAngles.x, transform.GetChild(0).eulerAngles.y + 2, transform.GetChild(0).eulerAngles.z);  
        }
    }
}

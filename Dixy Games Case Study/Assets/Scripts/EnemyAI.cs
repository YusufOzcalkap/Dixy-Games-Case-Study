using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI instance;

    [Header("Enemy Controller")]
    public float speedForward = 5;
    public float collectionspeed;
    public int speedRightLeft;
    public float bounders;
    private float _randomCount;
    private Vector3 _ballvector;
    public float _rotateSpeed;
    public int _characterCount;
    private bool _readyTrigger;
    private Rigidbody _rg;

    private float Count;
    private float rnd;
    void Start()
    {
        instance = this;
        _randomCount = 2;
        _characterCount = 1;
        SetSpeed();
        _rg = transform.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (GameManager.instance.gamestate == GameManager.GameState.InGame)
        {
            transform.GetChild(0).RotateAround(transform.GetChild(0).position, _ballvector, _rotateSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * speedForward * Time.deltaTime);
            Bounders();

            Count -= Time.deltaTime;
            if (Count <= 0)
            {
                rnd = Random.Range(-bounders, bounders);

                Count = 1;
            }

            if (Count > 0)
            {
                transform.position = transform.position + transform.right * rnd * speedRightLeft * Time.deltaTime;
            }
        }       
    }

    private void LateUpdate()
    {
        _randomCount += Time.deltaTime;
        if (_randomCount >= 2)
        {
            _ballvector = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            _randomCount = 0;
        }
    }

    public void Bounders()
    {
        Vector3 boundry = transform.position;
        boundry.x = Mathf.Clamp(boundry.x, -bounders, bounders);
        transform.position = boundry;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && !_readyTrigger)
        {
            StartCoroutine(SetWallTrigger());
            if (other.GetComponent<WallController>().wallstate == WallController.WallState.impact)
            {
                _characterCount *= other.GetComponent<WallController>()._number;
                SetSpeed();
            }

            if (other.GetComponent<WallController>().wallstate == WallController.WallState.divide)
            {
                _characterCount /= other.GetComponent<WallController>()._number;
                SetSpeed();
            }

            if (other.GetComponent<WallController>().wallstate == WallController.WallState.collection)
            {
                _characterCount += other.GetComponent<WallController>()._number;
                SetSpeed();
            }

            if (other.GetComponent<WallController>().wallstate == WallController.WallState.extraction)
            {
                _characterCount -= other.GetComponent<WallController>()._number;
                SetSpeed();
            }
        }

        if (other.gameObject.CompareTag("AddChar"))
        {
            _characterCount++;
            Destroy(other.gameObject);
            SetSpeed();
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            _characterCount--;
            SetSpeed();
        }

        if (other.gameObject.CompareTag("SpeedUp"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            speedForward += 2;
        }

        if (other.gameObject.CompareTag("SpeedDown"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            speedForward -= 2;
        }
    }

    IEnumerator SetWallTrigger()
    {
        _readyTrigger = true;
        yield return new WaitForSeconds(1f);
        _readyTrigger = false;
    }
    void SetSpeed()
    {
        collectionspeed = 0;
        collectionspeed = _characterCount / 10;
        speedForward += collectionspeed;
    }
}

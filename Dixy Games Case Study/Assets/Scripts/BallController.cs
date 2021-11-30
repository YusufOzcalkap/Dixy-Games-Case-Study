using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public static BallController instance;
    [Header("Player Controller")]
    private float mouseFirstPosX;
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

    [Header("Particle System")]
    public ParticleSystem[] _ps;
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
            if (Input.GetMouseButtonDown(0))
                mouseFirstPosX = Input.mousePosition.x;
            else if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x != mouseFirstPosX)
                    transform.position = transform.position + transform.right * (Input.mousePosition.x - mouseFirstPosX) / speedRightLeft * Time.deltaTime;        
            }
            Bounders();
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

    // limitation of the character's movements
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
            _ps[1].Play();
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            _characterCount--;
            SetSpeed();
            _rg.AddForce(transform.up * 300);
            _ps[0].Play();
        }
         
        if (other.gameObject.CompareTag("SpeedUp"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            speedForward += 3;
        }

        if (other.gameObject.CompareTag("SpeedDown"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            speedForward -= 0.5f;
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(SetFinish());
        }

        if (other.gameObject.CompareTag("BallFinish"))
        {
            other.GetComponent<Rigidbody>().AddExplosionForce(1100, other.transform.position, 15, 4.0f);
            Camera.main.transform.GetComponent<CameraController>().enabled = false;
            GameManager.instance.gamestate = GameManager.GameState.Next;
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
        collectionspeed = _characterCount / 20;
        speedForward += collectionspeed;
    }

    IEnumerator SetFinish()
    {
        yield return new WaitForSeconds(0.2f);
        speedForward = 0;
        yield return new WaitForSeconds(1f);
        transform.GetChild(1).gameObject.SetActive(false);
        _rg.AddForce(transform.forward * 1000);
    }
}

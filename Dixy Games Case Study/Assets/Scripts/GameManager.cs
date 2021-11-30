using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("GameController")]
    public TextMeshProUGUI _countText;
    public GameObject StartP, InGameP, NextP, GameOverP;
    public float CountDown = 2f;

    [SerializeField] private int asynSceneIndex = 1;

    public enum GameState
    {
        Start,
        InGame,
        Next,
        GameOver
    }

    public GameState gamestate;

    public enum Panels
    {
        Startp,
        Nextp,
        GameOverp,
        InGamep
    }
    public Image _finishbar;
    public GameObject _finish;
    private GameObject _Player;
    private GameObject _enemy;
    float _distanceBar;
    private void Start()
    {
        instance = this;
        SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
        gamestate = GameState.Start;
        _Player = GameObject.Find("Player");
        _enemy = GameObject.Find("PlayerEnemy");
    }

    private void Update()
    {
        _countText.text = BallController.instance._characterCount.ToString();

        _finish = GameObject.Find("Finish");
        float dist = Vector3.Distance(_finish.transform.position, _Player.transform.position);
        if (dist > 0) _distanceBar = (_finish.transform.position.z - dist) / _finish.transform.position.z;
        _finishbar.fillAmount = Mathf.Lerp(_finishbar.fillAmount, _distanceBar, 0.02f);

        // Enums and process
        switch (gamestate)
        {
            case GameState.Start: GameStart();
                break;
            case GameState.InGame: GameInGame();
                break;
            case GameState.Next: GameFinish();
                break;
            case GameState.GameOver: GameOver();
                break;
        }
    }

    void PanelController(Panels CurrentPanel)
    {
        StartP.SetActive(false);
        InGameP.SetActive(false);
        NextP.SetActive(false);
        GameOverP.SetActive(false);

        switch (CurrentPanel)
        {
            case Panels.Startp: StartP.SetActive(true);
                break;
            case Panels.InGamep: InGameP.SetActive(true);
                break;
            case Panels.Nextp: NextP.SetActive(true);
                break;
            case Panels.GameOverp: GameOverP.SetActive(true);
                break;
        }
    }

    void GameStart()
    {
        PanelController(Panels.Startp);
        if (SceneManager.sceneCount < 2) SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
    }

    void GameInGame()
    {
        PanelController(Panels.InGamep);
    }

    void GameFinish()
    {
        PanelController(Panels.Nextp);
    }

    void GameOver()
    {
        CountDown -= Time.deltaTime;
        if (CountDown <0 )
             PanelController(Panels.GameOverp );
    }

    public void RestartButton()
    {
        gamestate = GameState.Start;
        SceneManager.UnloadSceneAsync(asynSceneIndex);
        SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
    }

    public void NextLevelButton()
    {
        gamestate = GameState.Start;
        _Player.transform.GetChild(1).gameObject.SetActive(true);
        _Player.transform.position = new Vector3(0, 1, 0);
        BallController.instance._characterCount = 1;
        _enemy.transform.position = new Vector3(-2.49f, 1, 0);
        EnemyAI.instance._characterCount = 1;
        Camera.main.transform.position = new Vector3(0, 7.26f, -10);
        Camera.main.transform.GetComponent<CameraController>().enabled = true;

        if (SceneManager.sceneCountInBuildSettings == asynSceneIndex + 1)
        {
            SceneManager.UnloadSceneAsync(asynSceneIndex);
            asynSceneIndex = 1;
            SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
        }
        else
        {
            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync(asynSceneIndex);
                asynSceneIndex++;
            }
            SceneManager.LoadSceneAsync(asynSceneIndex, LoadSceneMode.Additive);
        }
    }

    public void StartButton()
    {
        gamestate = GameState.InGame;
        BallController.instance.speedForward = 4;
        EnemyAI.instance.speedForward = 4;
    }
}

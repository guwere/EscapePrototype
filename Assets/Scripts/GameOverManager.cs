using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    public ScoreText _currentScoreText;
    public ScoreText _targetScoreText;

    public float _restartDelay = 2f;

    private float _restartTimer;
    private RunnerSpawner _runnerSpawner;
    private Animator _anim;
    private GameConfiguration _gameConfig;

    private void Awake()
    {
        _gameConfig = (GameConfiguration) FindObjectOfType(typeof (GameConfiguration));
        _anim = GameObject.FindGameObjectWithTag("HUDCanvas").GetComponent<Animator>();
        _runnerSpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<RunnerSpawner>();
        _targetScoreText._score = _gameConfig._targetScore;
    }


    private void Update()
    {

        if (_currentScoreText._score >= _gameConfig._targetScore)
        {
            GameObject.Find("GameOverText").GetComponent<Text>().text = "You Win!";
            TransitionToGameOver();
        }
        else if (_currentScoreText._score +
                 _gameConfig._mouseScore * (_gameConfig._totalChasees + _runnerSpawner.ChaseeInPlay) <
                 _gameConfig._targetScore)
        {
            GameObject.Find("GameOverText").GetComponent<Text>().text = "You Lose!";

            TransitionToGameOver();
        }

    }

    private void TransitionToGameOver()
    {
        // ... tell the animator the game is over.
        _anim.SetTrigger("GameOver");

        // .. increment a timer to count up to restarting.
        _restartTimer += Time.deltaTime;

        // .. if it reaches the restart delay...
        if (_restartTimer >= _restartDelay)
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
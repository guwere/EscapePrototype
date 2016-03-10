using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    public ScoreText _currentScore;
    public ScoreText _targetScore;

    private Animator _anim;
    private float _restartTimer;
    public float _restartDelay = 2f;
    private RunnerSpawner _runnerSpawner;

    private void Awake()
    {
        // Set up the reference.
        _anim = GetComponent<Animator>();
        _runnerSpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<RunnerSpawner>();
    }


    private void Update()
    {

        if (_currentScore._score >= _targetScore._score)
        {
            GameObject.Find("GameOverText").GetComponent<Text>().text = "You Win!";
            TransitionToGameOver();
        }
        else if (_currentScore._score +
                 ChaseeController._exitScoreInitial * (_runnerSpawner._totalChasees + _runnerSpawner.ChaseeInPlay) <
                 _targetScore._score)
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
            // .. then reload the currently loaded level.
            SceneManager.LoadScene("GameScene");
        }
    }
}
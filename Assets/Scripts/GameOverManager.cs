using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public ScoreText _currentScore;
    public ScoreText _targetScore;

    private Animator _anim;
    private float _restartTimer;
    public float _restartDelay = 2f;


    void Awake()
    {
        // Set up the reference.
        _anim = GetComponent<Animator>();
    }


    void Update()
    {

        if (_currentScore._score >= _targetScore._score)
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
}
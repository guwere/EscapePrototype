using UnityEngine;
using System.Collections;

public class ChaseeController : RunnerController
{
    public static int _exitScoreInitial = 10;
    private RunnerSpawner _runnerSpawner;

    protected override void Start()
    {
        base.Start();
        _exitScoreCurrent = _exitScoreInitial;
        _runnerSpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<RunnerSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Chaser")
        {
            _originalColor = _failColor;
            _exitScoreCurrent = 0;
            _runnerState = State.eVanishing;
        }
    }

    protected override void reachedExitPoint()
    {
        base.reachedExitPoint();
        _runnerSpawner.ChaseeInPlay = _runnerSpawner.ChaseeInPlay - 1;
    }
}

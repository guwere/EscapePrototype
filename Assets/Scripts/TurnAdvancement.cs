using System;
using UnityEngine;
using System.Collections;

public class TurnAdvancement : MonoBehaviour
{
    public enum GameState
    {
        eIdle,
        eCalculatePositions,
        eMoving,
    }
    public GameState State
    {
        get { return _state; }
    }

    public float ElapsedTurnTime
    {
        get { return _elapsedTurnTime; }
    }

    public float _turnTime = 1f; // in seconds

    private GameState _state = GameState.eIdle;
    private float _elapsedTurnTime = 0f;

    // Use this for initialization
    void Start()
    {

    }


    void Update()
    {
        switch (_state)
        {
            case GameState.eIdle:
                {
                    if (Input.GetButtonDown("AdvanceTurn")) // wait for input
                    {
                        _state = GameState.eCalculatePositions;
                        _elapsedTurnTime = 0;
                    }

                }
                break;
            case GameState.eCalculatePositions:
                {
                    RunnerMovement[] runners = GameObject.FindObjectsOfType<RunnerMovement>();
                    foreach (var runner in runners)
                    {
                        Debug.Log("Direction before: " + runner.Direction);
                        runner.RespondToArrow();
                        Debug.Log("Direction after: " + runner.Direction);
                        runner.CalculateNextPosition();
                    }
                    _state = GameState.eMoving;
                }
                break;
            case GameState.eMoving:
                {
                    if (_elapsedTurnTime < _turnTime)
                    {
                        float percentageComplete = _elapsedTurnTime / _turnTime;

                        RunnerMovement[] runners = GameObject.FindObjectsOfType<RunnerMovement>();
                        foreach (var runner in runners)
                        {
                            runner.Move(percentageComplete);
                        }
                        _elapsedTurnTime += Time.deltaTime;
                    }
                    else
                    {
                        _state = GameState.eIdle;
                    }

                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}

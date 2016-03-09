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
        eCheckExitPoints,
        eChechEndGame
    }
    public GameState State
    {
        get { return _state; }
    }

    public float ElapsedTurnTime
    {
        get { return _elapsedTurnTime; }
    }

    public int TurnsElapsed
    {
        get { return _turnsElapsed; }
    }

    public float _turnDuration = 1f; // in seconds

    private GameState _state = GameState.eIdle;
    private float _elapsedTurnTime = 0f;
    private int _turnsElapsed = 0;


    void Start()
    {

    }


    void Update()
    {
        RunnerMovement[] runners = GameObject.FindObjectsOfType<RunnerMovement>();
        switch (_state)
        {
            case GameState.eIdle:
                {
                    if (Input.GetButtonDown("AdvanceTurn")) // wait for input
                    {
                        _state = GameState.eCalculatePositions;
                        _elapsedTurnTime = 0;
                        _turnsElapsed++;
                    }

                }
                break;
            case GameState.eCalculatePositions:
                {
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
                    if (_elapsedTurnTime < _turnDuration)
                    {
                        float percentageComplete = _elapsedTurnTime / _turnDuration;

                         foreach (var runner in runners)
                        {
                            runner.Move(percentageComplete);
                        }
                        _elapsedTurnTime += Time.deltaTime;
                    }
                    else
                    {
                        _state = GameState.eCheckExitPoints;
                    }

                }
                break;
            case GameState.eCheckExitPoints:
                {
                    foreach (var runner in runners)
                    {
                        runner.CheckExitPoint();
                    }
                    _state = GameState.eIdle;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}

using System;
using UnityEngine;
using System.Collections;

public class TurnAdvancement : MonoBehaviour
{
    public enum GameState
    {
        eIdle,
        eMoving,
        ePostMoving,
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
                        RunnerMovement[] runners = GameObject.FindObjectsOfType<RunnerMovement>();
                        foreach (var runner in runners)
                        {
                            runner.CalculateNextPosition();
                        }
                        _state = GameState.eMoving;
                        _elapsedTurnTime = 0;
                    }

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
            case GameState.ePostMoving:
                {

                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //if (Input.GetButtonDown("AdvanceTurn"))
        //{
        //    if (_state == GameState.eIdle)
        //    {
        //        _elapsedTurnTime = 0f;
        //        _state = GameState.eMoving;
        //    }
        //    else if (_state == GameState.eMoving)
        //    {
        //        _elapsedTurnTime += Time.deltaTime;

        //        if (_elapsedTurnTime >= _turnTime)
        //        {
        //            _state = GameState.eIdle;
        //        }

        //    }

        //}
    }
}

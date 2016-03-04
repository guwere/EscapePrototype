using UnityEngine;
using System.Collections;

public class TurnAdvancement : MonoBehaviour
{
    public enum GameState
    {
        eIdle,
        eMoving
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
    public float _speed = 2f;

    private GameState _state = GameState.eIdle;
    private float _elapsedTurnTime = 0f;

    // Use this for initialization
    void Start()
    {

    }


    void Update()
    {
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

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunnerController : MonoBehaviour
{
    public float _vanishDuration = 1f;
    private float _elapsedVanishDuration = 0f;
    private Color _originalColor;
    private Color _vanishColor = new Color(0, 0, 0, 0);
    public Color32 _successColor = new Color(128, 255, 128, 0);
    public Color32 _failColor = new Color(0, 0, 0, 0);
    public int _exitScore = 10;

    public enum State
    {
        eMoving,
        eVanishing,
        eDestroy
    }

    private FloorGridController _floorGridPlacer;

    private State _runnerState = State.eMoving;
    private Directions2d _direction = Directions2d.eNone;

    private Vector3 _startPosition;
    private Vector3 _nextPosition;

    public Directions2d Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public State RunnerState
    {
        get { return _runnerState; }
    }

    protected virtual void Start()
    {
        _originalColor = GetComponent<Renderer>().material.color;
        _floorGridPlacer = GameObject.FindGameObjectWithTag("Floor").GetComponent<FloorGridController>();
    }

    protected virtual void Update()
    {
        switch (_runnerState)
        {
            case State.eMoving:
                break;
            case State.eVanishing:
                if (_elapsedVanishDuration < _vanishDuration)
                {
                    float percentageComplete = _elapsedVanishDuration / _vanishDuration;
                    GetComponent<Renderer>().material.color = Color.Lerp(_originalColor, _vanishColor, percentageComplete);

                    _elapsedVanishDuration += Time.deltaTime;
                }
                else
                {
                    _runnerState = State.eDestroy;
                }
                break;
            case State.eDestroy:
                reachedExitPoint();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected virtual void reachedExitPoint()
    {
        GameObject.Find("ScoreText").GetComponent<ScoreText>().AddScore(_exitScore);
        Destroy(this.gameObject);
    }

    public void CalculateNextPosition()
    {
        GameObject currTile = GetTileBelow();
        GridPosition currGridPos = _floorGridPlacer.GetGridPosition(currTile);
        GridMovement nextMove = GetNextGridMovementDefault(currGridPos._row, currGridPos._col);
        GameObject nextTile = _floorGridPlacer.GetFloorTile(nextMove._position);
        _nextPosition = nextTile.transform.position;
        _nextPosition.y = transform.position.y;
        _startPosition = transform.position;
    }

    public void Move(float f)
    {
        if (RunnerState == State.eMoving)
        {
            transform.position = Vector3.Lerp(_startPosition, _nextPosition, f);
        }
    }

    private GameObject GetTileBelow()
    {
        RaycastHit hit;
        int mask = 0;
        mask |= (1 << LayerMask.NameToLayer("FloorTiles"));
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, mask))
        {
            return hit.transform.gameObject;
        }
        else
        {
            Debug.LogError("Runner is not on top of any tile");
            Debug.Log(UnityEngine.StackTraceUtility.ExtractStackTrace());
        }
        return null;
    }

    private GridMovement GetNextGridMovementDefault(int iRow, int iCol)
    {
        GridMovement result = new GridMovement
        {
            _position =
            {
                _row = iRow, _col = iCol
            },
            _direction = _direction
        };

        switch (result._direction)
        {
            case Directions2d.eNone:
                break;
            case Directions2d.eUp:
            {
                if (result._position._row < _floorGridPlacer._rows - 1)
                {
                    result._position._row++;
                }
                else
                {
                    if (result._position._col < _floorGridPlacer._columns - 1)
                    {
                        result._position._col++;
                        _direction = Directions2d.eRight;
                    }
                    else
                    {
                        result._position._row--;
                        _direction = Directions2d.eDown;
                    }
                }
            }
                break;
            case Directions2d.eDown:
            {
                if (result._position._row > 0)
                {
                    result._position._row--;
                }
                else
                {
                    if (result._position._col > 0)
                    {
                        result._position._col--;
                        _direction = Directions2d.eLeft;
                    }
                    else
                    {
                        result._position._row++;
                        _direction = Directions2d.eUp;
                    }
                }
            }

                break;
            case Directions2d.eLeft:
            {
                if (result._position._col > 0)
                {
                    result._position._col--;
                }
                else
                {
                    if (result._position._row < _floorGridPlacer._rows - 1)
                    {
                        result._position._row++;
                        _direction = Directions2d.eUp;
                    }
                    else
                    {
                        result._position._col++;
                        _direction = Directions2d.eRight;
                    }
                }
            }
                break;
            case Directions2d.eRight:
            {
                if (result._position._col < _floorGridPlacer._columns - 1)
                {
                    result._position._col++;
                }
                else
                {
                    if (result._position._row > 0)
                    {
                        result._position._row--;
                        _direction = Directions2d.eDown;
                    }
                    else
                    {
                        result._position._col--;
                        _direction = Directions2d.eLeft;
                    }
                }
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return result;
    }


    public void RespondToArrow()
    {
        FloorTileController tile = GetTileBelow().GetComponent<FloorTileController>();
        Directions2d newDirection = tile.ArrowDirection;
        if (newDirection != Directions2d.eNone)
        {
            _direction = newDirection;
        }
    }

    public void CheckExitPoint()
    {
        bool isOnExitPoint = GetTileBelow().GetComponent<FloorTileController>().IsExitPoint;
        if (isOnExitPoint && RunnerState == State.eMoving)
        {
            _vanishColor = _successColor;
            _runnerState = State.eVanishing;
        }
    }
}

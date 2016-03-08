using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunnerMovement : MonoBehaviour
{
    private GameObject _gameController;
    private TurnAdvancement _turn;
    private FloorGridPlacer _floorGridPlacer;

    private Directions2d _direction = Directions2d.eNone;

    private Vector3 _startPosition;
    private Vector3 _nextPosition;

    public Directions2d Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController");
        _turn = _gameController.GetComponent<TurnAdvancement>();
        _floorGridPlacer = GameObject.FindGameObjectWithTag("Floor").GetComponent<FloorGridPlacer>();
    }

    void Update()
    {

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
        //Debug.Log("Start pos : (" + currGridPos._row + "," + currGridPos._col + ")");
        //Debug.Log("End pos : (" + nextMove._position._row + "," + nextMove._position._col + ")");

    }

    public void Move(float f)
    {
        transform.position = Vector3.Lerp(_startPosition, _nextPosition, f);
    }

    private GameObject GetTileBelow()
    {
        RaycastHit hit;
        int mask = 0;
        mask |= (1 << LayerMask.NameToLayer("FloorTiles"));
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, mask))
        {
            Debug.Log("The name of the object: " + hit.transform.gameObject.tag);
            return hit.transform.gameObject;
        }
        else
        {
            Debug.LogError("Runner is not on top of any tile");
        }
        return null;
    }

    GridMovement GetNextGridMovementDefault(int iRow, int iCol)
    {
        GridMovement result = new GridMovement
        {
            _position =
            {
                _row = iRow,
                _col = iCol
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
        SingleTileManager tile = GetTileBelow().GetComponent<SingleTileManager>();
        Directions2d newDirection = tile.ArrowDirection;
        if (newDirection != Directions2d.eNone)
        {
            _direction = newDirection;
        }
    }
}

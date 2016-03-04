using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct GridMovement
{
    public GridPosition _position;
    public Directions2d _direction;
}


public class RunnerMovement : MonoBehaviour
{
    private GameObject _gameController;
    private TurnAdvancement _turn;
    private FloorGridPlacer _floorGridPlacer;

    private Directions2d _direction = Directions2d.eNone;

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
        if (Input.GetButtonDown("AdvanceTurn"))
        {
            Debug.Log("Advance Turn Pressed");
            GameObject currTile = GetTileBelow();
            GridPosition currGridPos = _floorGridPlacer.GetGridPosition(currTile);
            GridMovement nextMove = GetNextGridMovementDefault(currGridPos._row, currGridPos._col);
            GameObject nextTile = _floorGridPlacer.GetFloorTile(nextMove._position);
            Vector3 nextWorldPos = nextTile.transform.position;
            nextWorldPos.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, nextWorldPos, 2.0f);
        }
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
}

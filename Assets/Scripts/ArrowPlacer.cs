using System;
using UnityEngine;
using System.Collections;

public enum Directions2d
{
    eNone,
    eUp,
    eDown,
    eLeft,
    eRight
};

public class ArrowPlacer : MonoBehaviour
{

    public GameObject _arrowPlanePrefab;
    public float _removeArrowThreshold = 5.0f; // the distance between the start and end touch locations

    private Vector3 _startPosition;
    private Directions2d _direction;
    private int _row;
    private int _col;
    private GameObject _currArrow;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _startPosition = Input.mousePosition;
            _direction = Directions2d.eNone;

        }
        else if (Input.GetButtonUp("Fire1"))
        {
        }

    }

    void SpawnArrow()
    {
        if (!_currArrow)
        {
            _currArrow = Instantiate(_arrowPlanePrefab);
            _currArrow.transform.parent = this.transform;
            _currArrow.transform.localPosition = new Vector3(0, .05f, 0);
        }

        Vector3 localRotation = new Vector3(90.0f, 0, 0);
        switch (_direction)
        {
            case Directions2d.eUp:
                localRotation.z = 0.0f;
                break;
            case Directions2d.eDown:
                localRotation.z = 180.0f;
                break;
            case Directions2d.eLeft:
                localRotation.z = 90.0f;
                break;
            case Directions2d.eRight:
                localRotation.z = 270.0f;
                break;
            case Directions2d.eNone:
                {
                    if (_currArrow)
                    {
                        Destroy(_currArrow);
                    }
               }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _currArrow.transform.localEulerAngles = localRotation;
    }

    private void OnMouseUp()
    {
        Debug.Log("Tile: (" + _row + "," + _col + ") was pressed");
        Vector3 endPosition = Input.mousePosition;
        _direction = GetDirection2d(_startPosition, endPosition);
        SpawnArrow();
    }

    public void SetGridPosition(int row, int col)
    {
        _row = row;
        _col = col;
    }

    private Directions2d GetDirection2d(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 direction = endPosition - _startPosition;
        Directions2d result = Directions2d.eNone;

        Debug.Log("The size of the vector is: " + direction.magnitude);

        if (direction.magnitude < _removeArrowThreshold)
        {
            result = Directions2d.eNone;
        }
        else
        {
            direction.Normalize();

            float f = Vector3.Dot(direction, Vector3.up);
            if (f >= 0.5)
            {
                result = Directions2d.eUp;
            }
            else if (f <= -0.5)
            {
                result = Directions2d.eDown;
            }
            else
            {
                f = Vector3.Dot(direction, Vector3.right);
                if (f >= 0.5)
                {
                    result = Directions2d.eRight;
                }
                else if (f <= -0.5)
                {
                    result = Directions2d.eLeft;
                }
            }
         }

        return result;

    }

}

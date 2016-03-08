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

public class SingleTileManager : MonoBehaviour
{

    public GameObject _arrowPlanePrefab;
    public float _removeArrowThreshold = 5.0f; // the distance between the start and end touch locations

    private GridPosition _gridPosition;
    private Vector3 _startPosition;

    private Directions2d _arrowDirection;  
    private GameObject _currArrow;

    public GridPosition GridPosition
    {
        get { return _gridPosition; }
        set { _gridPosition = value; }
    }

    public Directions2d ArrowDirection
    {
        get { return _arrowDirection; }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        _startPosition = Input.mousePosition;
        _arrowDirection = Directions2d.eNone;
    }

    private void OnMouseUp()
    {
        Vector3 endPosition = Input.mousePosition;
        _arrowDirection = GetDirection2d(_startPosition, endPosition);
        SpawnArrow();
    }

    void SpawnArrow()
    {
        if (_currArrow)
        {
            Destroy(_currArrow);
        }

        Vector3 localRotation = new Vector3(90.0f, 0, 0);
        switch (_arrowDirection)
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
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (!_currArrow && _arrowDirection != Directions2d.eNone)
        {
            _currArrow = Instantiate(_arrowPlanePrefab);
            _currArrow.transform.parent = this.transform;
            _currArrow.transform.localPosition = new Vector3(0, .05f, 0);
        }
        _currArrow.transform.localEulerAngles = localRotation;
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

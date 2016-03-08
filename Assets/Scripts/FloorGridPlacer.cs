using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;


public class FloorGridPlacer : MonoBehaviour
{
    [Range(1, 32)]
    public int _rows = 10;
    [Range(1, 32)]
    public int _columns = 8;
    [Range (0.0f, 1.0f)]
    public float _tileSpacing = 0.1f; // percent fo the size of the tile

    public int _numHoles = 5;

    public GameObject _groundTile;
    public GameObject _tileOrigin;
    
    public GameObject _mouseHole;
    public GameObject [] _baseboards;
    public float _exitPointAlpha = 100f;

    private ArrayList _exitPoints;

    private GameObject[][] _floorTiles;

    private Vector3 _floorSize;

    private int[] _wallOccupanies; // we will use the int as a 32-bit mask

    private Vector3 _tileScaleFactorVector;
    private Vector3 _groundTileScale;

    public Vector3 TileScaleFactorVector
    {
        get { return _tileScaleFactorVector; }
    }

    public Vector3 GroundTileScale
    {
        get { return _groundTileScale; }
    }


    void Awake ()
	{
        _floorSize = GetComponent<Renderer>().bounds.size;
        _tileScaleFactorVector = new Vector3((_floorSize.z/_columns), 1f, (_floorSize.x / _rows));

        int numWalls = Enum.GetNames(typeof(WallSide)).Length;
        _wallOccupanies = new int[numWalls];

	    _floorTiles = new GameObject[_columns][];
        for (int col = 0; col < _columns; col++)
        {
            _floorTiles[col] = new GameObject[_rows];
        }

        _exitPoints = new ArrayList();

        CreateGroundTiles();
	    CreateMouseHoles();
	}

    void CreateGroundTiles()
    {
        // Change the size of the tile
        _groundTileScale = _groundTile.transform.localScale;
        _groundTileScale.z = (_floorSize.z / _columns) * (1.0f - _tileSpacing);
        _groundTileScale.x = _floorSize.x / _rows * (1.0f - _tileSpacing);
        // Get the tile size
        Vector3 anchorOffset = new Vector3(-_tileScaleFactorVector.z / 2, 0, _tileScaleFactorVector.x / 2);


        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                GameObject groundTile = Instantiate(_groundTile, new Vector3(0, 0), Quaternion.identity) as GameObject;
                // Change the parent
                groundTile.transform.parent = _tileOrigin.transform;
                groundTile.transform.localScale = _groundTileScale;

                Vector3 currPosition = new Vector3(row * -_tileScaleFactorVector.z, 0, col * _tileScaleFactorVector.x);
                groundTile.transform.localPosition = currPosition + anchorOffset;
                _floorTiles[col][row] = groundTile;
                SingleTileManager floorTile = groundTile.GetComponent<SingleTileManager>();
                floorTile.GridPosition = new GridPosition(col, row);
            }
        }
    }

    void CreateMouseHoles()
    {
        float mouseHoleYOffset = _mouseHole.GetComponent<Renderer>().bounds.size.y / 2;
        //Random.seed = 101;

        for (int i = 0; i < _numHoles; i++)
        {
            // Chooose a random wall
            int wallNum = Random.Range(0, Enum.GetNames(typeof(WallSide)).Length);
            WallSide wallSideEnum = (WallSide) wallNum;
            bool horizontal = (WallSide)wallNum == WallSide.eBack;

            // Instantiate hole
            GameObject mouseHole = Instantiate(_mouseHole);

            // Change the position of the hole
            mouseHole.transform.parent = _tileOrigin.transform;

            // Initialise default transform properties
            Vector3 mouseHolePosition = new Vector3(0f, mouseHoleYOffset, 0f);
            mouseHolePosition -= _tileOrigin.transform.localPosition;
            Vector3 mouseHoleRotation = new Vector3(0f, 0f, 0f);
            Vector3 mouseHoleScale = new Vector3(horizontal ? _groundTileScale.z : _groundTileScale.x, mouseHole.transform.localScale.y, mouseHole.transform.localScale.z);
            switch (wallSideEnum)
            {
                case WallSide.eLeft:
                    mouseHolePosition.z += -_floorSize.z / 2;
                    break;
                case WallSide.eRight:
                    mouseHolePosition.z += _floorSize.z / 2;
                    break;
                case WallSide.eBack:
                    mouseHolePosition.x += (-_floorSize.x / 2);
                    mouseHoleRotation.y = 90f;
                    break;
            }

            mouseHole.transform.Rotate(mouseHoleRotation);
            mouseHole.transform.localScale = mouseHoleScale;

            // Change where the mouse hole is positioned along the wall
            int wallOccupancy = _wallOccupanies[wallNum];
            bool placed = false;
            int randomPosition = 0;
            while (!placed)
            {
                randomPosition = Random.Range(0, (horizontal ? _columns : _rows));
                Debug.Log("Random position(" + ((WallSide)wallNum).ToString() + "," + randomPosition + ")");
                if ((wallOccupancy & (1 << randomPosition)) == 0)
                {
                    _wallOccupanies[wallNum] = wallOccupancy | (1 << randomPosition);
                    placed = true;
                    if (horizontal)
                    {
                        mouseHolePosition.z = _floorTiles[randomPosition][0].transform.localPosition.z;
                    }
                    else
                    {
                        mouseHolePosition.x = _floorTiles[0][randomPosition].transform.localPosition.x;
                    }

                }
            }
            mouseHole.transform.localPosition = mouseHolePosition;

            GridPosition gridPosition = GetMouseHoleGridPosition(wallSideEnum, randomPosition);
            MarkExitPoint(gridPosition);
        }

    }

    private void MarkExitPoint(GridPosition gridPosition)
    {
        GameObject tile = _floorTiles[gridPosition._col][gridPosition._row];
        Color newColor = tile.GetComponent<Renderer>().materials[0].color;
        newColor.a = _exitPointAlpha / 255f;
        tile.GetComponent<Renderer>().materials[0].color = newColor;

        if (!_exitPoints.Contains(tile))
        {
            _exitPoints.Add(tile);
        }
    }

    private GridPosition GetMouseHoleGridPosition(WallSide wallSideEnum, int randomPosition)
    {
        GridPosition result = new GridPosition();
        switch (wallSideEnum)
        {
            case WallSide.eLeft:
                result._row = randomPosition;
                result._col = 0;
                break;
            case WallSide.eRight:
                result._row = randomPosition;
                result._col = _columns - 1;
                break;
            case WallSide.eBack:
                result._row = _rows - 1;
                result._col = randomPosition;
                break;
            default:
                throw new ArgumentOutOfRangeException("wallSideEnum", wallSideEnum, null);
        }
        return result;
    }

    public GameObject GetFloorTile(GridPosition position)
    {
        return _floorTiles[position._col][position._row];
    }

    public GridPosition GetGridPosition(GameObject currTile)
    {
        for (int i = 0; i < _floorTiles.Length; i++)
        {
            for (int j = 0; j < _floorTiles[i].Length; j++)
            {
                if (_floorTiles[i][j] == currTile)
                {
                    return new GridPosition(i, j);
                }
            }
        }

        return new GridPosition(-1, -1);
    }
}

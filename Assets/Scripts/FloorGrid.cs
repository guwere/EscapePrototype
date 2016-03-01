using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class FloorGrid : MonoBehaviour
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

    private GameObject[][] groundTiles;

    private Vector3 _mouseHoleLocalPosition;
    private Vector3 _floorSize;

    private int[] _wallOccupanies; // we will use the int as a 32-bit mask

    private enum WallSide
    {
        eLeft,
        eRight,
        eBack
    }

	void Awake ()
	{
        _mouseHoleLocalPosition = new Vector3(0f, 0f, 0.3f);
        _floorSize = GetComponent<Renderer>().bounds.size;

	    int numWalls = Enum.GetNames(typeof(WallSide)).Length;
        _wallOccupanies = new int[numWalls];

	    groundTiles = new GameObject[_rows][];
        for (int col = 0; col < _columns; col++)
        {
            groundTiles[col] = new GameObject[_rows];
        }

        createGroundTiles();
	    createMouseHoles();
	}

    void createGroundTiles()
    {
        float tilePositionOffsetZ = (_floorSize.z / _columns);
        float tilePositionOffsetX = _floorSize.x / _rows;

        // Change the size of the tile
        Vector3 groundTileScale = _groundTile.transform.localScale;
        groundTileScale.z = (_floorSize.z / _columns) * (1.0f - _tileSpacing);
        groundTileScale.x = _floorSize.x / _rows * (1.0f - _tileSpacing);
        // Get the tile size
        Vector3 anchorOffset = new Vector3(-tilePositionOffsetX / 2, 0, tilePositionOffsetZ / 2);


        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                GameObject groundTile = Instantiate(_groundTile, new Vector3(0, 0), Quaternion.identity) as GameObject;
                // Change the parent
                groundTile.transform.parent = _tileOrigin.transform;
                groundTile.transform.localScale = groundTileScale;

                Vector3 currPosition = new Vector3(row * -tilePositionOffsetX, 0, col * tilePositionOffsetZ);
                groundTile.transform.localPosition = currPosition + anchorOffset;
                groundTiles[col][row] = groundTile;

            }
        }
    }

    void createMouseHoles()
    {
        float mouseHoleYOffset = _mouseHole.GetComponent<Renderer>().bounds.size.y / 2;
        //Random.seed = 101;

        for (int i = 0; i < _numHoles; i++)
        {
            // Chooose a random wall
            int wallNum = Random.Range(0, Enum.GetNames(typeof(WallSide)).Length);
            bool horizontal = (WallSide)wallNum == WallSide.eBack;

            // Instantiate hole
            GameObject mouseHole = Instantiate(_mouseHole);

            // Change the position of the hole
            mouseHole.transform.parent = _tileOrigin.transform;

            // Initialise default transform properties
            Vector3 mouseHolePosition = new Vector3(0f, mouseHoleYOffset, 0f);
            mouseHolePosition -= _tileOrigin.transform.localPosition;
            Vector3 mouseHoleRotation = new Vector3(0f, 0f, 0f);
            Vector3 mouseHoleScale = new Vector3((_floorSize.x / _rows) * (1.0f - _tileSpacing), mouseHole.transform.localScale.y, mouseHole.transform.localScale.z);
            switch ((WallSide)wallNum)
            {
                case WallSide.eLeft:
                    mouseHolePosition.z += -_floorSize.z / 2;
                    break;
                case WallSide.eRight:
                    mouseHolePosition.z += _floorSize.z / 2;
                    break;
                case WallSide.eBack:
                    mouseHolePosition.x += (-_floorSize.x / 2) + 0.3f;
                    mouseHoleRotation.y = 90f;
                    break;
            }

            mouseHole.transform.Rotate(mouseHoleRotation);
            mouseHole.transform.localScale = mouseHoleScale;

            // Change where the mouse hole is positioned along the wall
            int wallOccupancy = _wallOccupanies[wallNum];
            bool placed = false;
            while (!placed)
            {
                int randomPosition = Random.Range(0, (horizontal ? _columns : _rows));
                Debug.Log("Random position(" + ((WallSide)wallNum).ToString() + "," + randomPosition + ")");
                if ((wallOccupancy & (1 << randomPosition)) == 0)
                {
                    _wallOccupanies[wallNum] = wallOccupancy | (1 << randomPosition);
                    placed = true;
                    if (horizontal)
                    {
                        mouseHolePosition.z = groundTiles[randomPosition][0].transform.localPosition.z;
                    }
                    else
                    {
                        mouseHolePosition.x = groundTiles[0][randomPosition].transform.localPosition.x;
                    }

                }
            }
            mouseHole.transform.localPosition = mouseHolePosition;

        }
    }
}

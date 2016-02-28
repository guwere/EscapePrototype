using UnityEngine;
using System.Collections;

public class FloorGrid : MonoBehaviour
{

    public int _rows = 10;
    public int _columns = 8;
    [Range (0.0f, 1.0f)]
    public float _tileSpacing = 0.1f; // percent fo the size of the tile

    public GameObject _groundTile;
    public GameObject _tileOrigin;

	void Awake ()
	{
	    createGroundTiles();
	}

    void createGroundTiles()
    {
        Vector3 floorSize = GetComponent<Renderer>().bounds.size;
        float tilePositionOffsetX = (floorSize.x / _columns);
        float tilePositionOffsetZ = floorSize.z / _rows;

        // Change the size of the tile
        Vector3 groundTileScale = _groundTile.transform.localScale;
        groundTileScale.x = (floorSize.x / _columns) * (1.0f - _tileSpacing);
        groundTileScale.z = floorSize.z / _rows * (1.0f - _tileSpacing);
        // Get the tile size
        Vector3 anchorOffset = new Vector3(-tilePositionOffsetX / 2, 0, tilePositionOffsetZ / 2);


        for (int col = 0; col < _columns; col++)
        {
            for (int row = 0; row < _rows; row++)
            {
                GameObject groundTile = Instantiate(_groundTile, new Vector3(0, 0), Quaternion.identity) as GameObject;
                // Change the parent
                groundTile.transform.parent = _tileOrigin.transform;
                groundTile.transform.localScale = groundTileScale;

                Vector3 currPosition = new Vector3(col * -tilePositionOffsetX, 0, row * tilePositionOffsetZ);
                groundTile.transform.localPosition = currPosition + anchorOffset;


            }
        }
    }


}

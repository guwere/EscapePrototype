using UnityEngine;
using System.Collections;

public class FloorGrid : MonoBehaviour
{

    public int _rows = 10;
    public int _columns = 8;

    public GameObject _groundTile;
    public GameObject _tileOrigin;

	void Awake ()
	{
        Vector3 floorSize = GetComponent<Renderer>().bounds.size;
	    float tileSizeX = floorSize.x/_columns;
        float tileSizeZ = floorSize.z/_rows;

        // Change the size of the tile
        Vector3 groundTileScale = _groundTile.transform.localScale;
        groundTileScale.x = tileSizeX;
        groundTileScale.z = tileSizeZ;
        // Get the tile size
        Vector3 anchorOffset = new Vector3(-tileSizeX / 2, 0, tileSizeZ / 2);


        for (int col = 0; col < _columns; col++)
	    {
	        for (int row = 0; row < _rows; row++)
	        {
                GameObject groundTile = Instantiate(_groundTile, new Vector3(0, 0), Quaternion.identity) as GameObject;
                // Change the parent
                groundTile.transform.parent = _tileOrigin.transform;
                groundTile.transform.localScale = groundTileScale;

	            Vector3 currPosition = new Vector3(col * -groundTileScale.x, 0, row * groundTileScale.z);
	            groundTile.transform.localPosition = anchorOffset + currPosition;


	        }
        }
	}

}

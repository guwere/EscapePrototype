using UnityEngine;
using System.Collections;

public struct GridPosition
{
    public int _row;
    public int _col;

    public GridPosition(int col, int row) : this()
    {
        _row = row;
        _col = col;
    }
}

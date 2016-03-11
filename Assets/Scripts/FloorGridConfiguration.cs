using System;
using UnityEngine;
using System.Collections;

public class FloorGridConfiguration : MonoBehaviour
{

    [Range(1, 32)]
    public int _rows = 10;
    [Range(1, 32)]
    public int _columns = 8;

    public int _numHoles = 5;

    public void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

}

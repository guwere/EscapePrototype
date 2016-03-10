﻿using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;

public class RunnerSpawner : MonoBehaviour
{
    private FloorGridController _floor;
    private TurnAdvancement _turn;
    public GameObject _chaseePrefab;
    public GameObject _chaserPrefab;

    public int _chaseeSpawnFrequency = 5; // how many turns before the next batch of chasees are spawned
    public int _chaseeToSpawnMin = 1; // the minimum amount of chasee that will be spanwed
    public int _chaseeToSpawnMax = 2; // the maximum amount of chasee that will be spanwed
    public int _totalChasees = 50; // total number of chasees that can spawn
    private int _chaseeInPlay;

    public int _chaserToSpawnMin = 1;
    public int _chaserToSpawnMax = 2;
    public int _totalChasers = 10;


    private bool _allowChaserSpawn = false;



    private int _lastTurn = -1;

    public int ChaseeInPlay
    {
        get { return _chaseeInPlay; }
        set { _chaseeInPlay = value; }
    }

    // Use this for initialization
    void Start()
    {
        _floor = GameObject.FindGameObjectWithTag("Floor").GetComponent<FloorGridController>();
        _turn = GetComponent<TurnAdvancement>();

        //spawnRunnerTest();
    }

    void Update()
    {
        int currTurn = _turn.TurnsElapsed;
        if (_lastTurn < currTurn)// && currTurn % _chaseeSpawnFrequency == 0)
        {
            if (currTurn % _chaseeSpawnFrequency == 0 && _totalChasees > 0)
            {
                int chaseeToSpawn = Random.Range(_chaseeToSpawnMin, _chaseeToSpawnMax);
                chaseeToSpawn = Mathf.Clamp(chaseeToSpawn, 0, _totalChasees);
                _totalChasees -= chaseeToSpawn;
                while (chaseeToSpawn > 0)
                {
                    SpawnRunner(_chaseePrefab);
                    chaseeToSpawn--;
                    _chaseeInPlay++;
                }
                _allowChaserSpawn = true;
            }
            else if (_allowChaserSpawn && currTurn % (_chaseeSpawnFrequency + Random.Range(1, _chaseeSpawnFrequency + 1)) == 0 && _totalChasers > 0)
            {
                int chaserToSpawn = Random.Range(_chaserToSpawnMin, _chaserToSpawnMax);
                chaserToSpawn = Mathf.Clamp(chaserToSpawn, 0, _totalChasers);
                _totalChasers -= chaserToSpawn;
                while (chaserToSpawn > 0)
                {
                    SpawnRunner(_chaserPrefab);
                    chaserToSpawn--;
                }
                _allowChaserSpawn = false;
                
            }

            _lastTurn = currTurn;
        }
    }

    private void SpawnRunner(GameObject prefab)
    {
        GridMovement randomMovement = GetRandomGridMovement();

        GameObject floorTile = _floor.GetFloorTile(randomMovement._position);
        GameObject runner = Instantiate(prefab);
        runner.transform.localPosition = floorTile.transform.position + new Vector3(0, 0.2f, 0);
        runner.transform.localScale = new Vector3(runner.transform.localScale.x * floorTile.transform.localScale.x,
            runner.transform.localScale.y,
            runner.transform.localScale.z * floorTile.transform.localScale.z);
        runner.GetComponent<RunnerController>().Direction = randomMovement._direction;
    }

    public GridMovement GetRandomGridMovement()
    {
        GridMovement randomMovement = new GridMovement();
        int randomWall = 0;
        do
        {
            randomWall = Random.Range(0, Enum.GetNames(typeof(WallSide)).Length);
        }
        while (_floor.ExitPoints[randomWall].Count <= 0);

        int randomPosition = (int)_floor.ExitPoints[randomWall][Random.Range(0, _floor.ExitPoints[randomWall].Count)];
        randomMovement._position = _floor.GetMouseHoleGridPosition((WallSide)randomWall, randomPosition);
        randomMovement._direction = _floor.GetExitPointDirection((WallSide)randomWall);

        return randomMovement;
    }

    private void SpawnRunnerTest()
    {
        GridMovement randomMovement = new GridMovement();
        randomMovement._position._row = Random.Range(0, _floor._rows - 1);
        randomMovement._position._col = Random.Range(0, _floor._columns - 1);
        randomMovement._direction = Directions2d.eRight;

        GameObject floorTile = _floor.GetFloorTile(randomMovement._position);
        GameObject runner = Instantiate(_chaseePrefab);
        runner.transform.localPosition = floorTile.transform.position + new Vector3(0, 0.2f, 0);
        runner.transform.localScale = new Vector3(runner.transform.localScale.x * floorTile.transform.localScale.x,
            runner.transform.localScale.y,
            runner.transform.localScale.z * floorTile.transform.localScale.z);
        runner.GetComponent<RunnerController>().Direction = Directions2d.eRight;
    }

    public static int Clamp(int value, int min, int max)
    {
        return (value < min) ? min : (value > max) ? max : value;
    }
}

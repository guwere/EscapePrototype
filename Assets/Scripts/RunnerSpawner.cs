using UnityEngine;
using System.Collections;

public class RunnerSpawner : MonoBehaviour
{
    private FloorGridPlacer _floorGridPlacer;
    public GameObject _runnerPrefab;

    // Use this for initialization
    void Start()
    {
        _floorGridPlacer = GameObject.FindGameObjectWithTag("Floor").GetComponent<FloorGridPlacer>();

        spawnRunnerTest();
    }

    private void spawnRunnerTest()
    {
        GridMovement randomMovement = new GridMovement();
        randomMovement._position._row = Random.Range(0, _floorGridPlacer._rows - 1);
        randomMovement._position._col = Random.Range(0, _floorGridPlacer._columns - 1);
        randomMovement._direction = Directions2d.eRight;

        GameObject floorTile = _floorGridPlacer.GetFloorTile(randomMovement._position);
        GameObject runner = Instantiate(_runnerPrefab);
        runner.transform.localPosition = floorTile.transform.position + new Vector3(0, 0.2f, 0);
        runner.transform.localScale = new Vector3(runner.transform.localScale.x * floorTile.transform.localScale.x,
            runner.transform.localScale.y,
            runner.transform.localScale.z * floorTile.transform.localScale.z);
        runner.GetComponent<RunnerMovement>().Direction = Directions2d.eRight;
    }
}

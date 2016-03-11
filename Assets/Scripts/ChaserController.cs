using UnityEngine;
using System.Collections;

public class ChaserController : RunnerController
{

    protected override void Start()
    {
        base.Start();
        _exitScoreCurrent = ((GameConfiguration)FindObjectOfType(typeof(GameConfiguration)))._catScore;
    }
}

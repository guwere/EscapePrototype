using UnityEngine;
using System.Collections;

public class ChaserController : RunnerController
{

    protected override void Start()
    {
        base.Start();
        _exitScoreCurrent = ((GameOverManager)FindObjectOfType(typeof(GameOverManager)))._catScore;
    }
}

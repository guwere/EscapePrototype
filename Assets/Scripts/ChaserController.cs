using UnityEngine;
using System.Collections;

public class ChaserController : RunnerController
{
    public static int _exitScoreInitial = -20;

    protected override void Start()
    {
        base.Start();
        _exitScoreCurrent = _exitScoreInitial;
    }
}

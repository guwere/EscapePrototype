using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreText : MonoBehaviour
{
    public int _score = 0;
    public string _scoreFormat = "Score: {0}";

    private Text _scoreText;


    void Awake()
    {
        _scoreText = GetComponent<Text>();
    }


    void Update()
    {
        _scoreText.text = string.Format(_scoreFormat, _score);
    }
}

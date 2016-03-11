using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameConfiguration : MonoBehaviour
{

    [Range(1, 32)]
    public int _rows = 6;
    [Range(1, 32)]
    public int _columns = 6;

    public int _numHoles = 2;

    public int _mouseScore = 10;
    public int _catScore = -20;
    public int _targetScore = 400;

    public int _chaseeSpawnFrequency = 5; // how many turns before the next batch of chasees are spawned
    public int _totalChasees = 50; // total number of chasees that can spawn
    public int _totalChasers = 10;



    public void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void ChangeIntParameter(out int param, string inputFieldName)
    {
        param = Convert.ToInt32(GameObject.Find(inputFieldName).GetComponent<InputField>().text);
    }

    private void ChangeIntTextField(int param, string inputFieldName)
    {
        GameObject.Find(inputFieldName).GetComponent<InputField>().text = Convert.ToString(param);
    }

    public void OnDefaultsPressed()
    {
        ChangeIntTextField(6, "RowsInputField");
        ChangeIntTextField(6, "ColumnsInputField");
        ChangeIntTextField(2, "HolesInputField");
        ChangeIntTextField(10, "MousePointsInputField");
        ChangeIntTextField(-20, "CatPointsInputField");
        ChangeIntTextField(400, "TargetScoreInputField");
        ChangeIntTextField(50, "TotalMiceInputField");
        ChangeIntTextField(10, "TotalCatsInputField");
        ChangeIntTextField(2, "MouseSpawnFrequencyInputField");
    }


    public void OnRowsEndEdit()
    {
        ChangeIntParameter(out _rows, "RowsInputField");
    }
    public void OnColumnsEndEdit()
    {
        ChangeIntParameter(out _columns, "ColumnsInputField");
    }
    public void OnHolesEndEdit()
    {
        ChangeIntParameter(out _numHoles, "HolesInputField");
    }
    public void OnMousePointsEndEdit()
    {
        ChangeIntParameter(out _mouseScore, "MousePointsInputField");
    }
    public void OnCatPointsEndEdit()
    {
        ChangeIntParameter(out _catScore, "CatPointsInputField");
    }
    public void OnTargetScoreEndEdit()
    {
        ChangeIntParameter(out _targetScore, "TargetScoreInputField");
    }
    public void OnTotalMiceEndEdit()
    {
        ChangeIntParameter(out _totalChasees, "TotalMiceInputField");
    }
    public void OnTotalCatsEndEdit()
    {
        ChangeIntParameter(out _totalChasers, "TotalCatsInputField");
    }
    public void OnMouseSpawnFrequencyEndEdit()
    {
        ChangeIntParameter(out _chaseeSpawnFrequency, "MouseSpawnFrequencyInputField");
    }
}

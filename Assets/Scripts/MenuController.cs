﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnStartGamePressed()
    {
        Debug.Log("Start Game Pressed");
        SceneManager.LoadScene("GameScene");
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public GameObject HighScoreGUI;
    public GameObject TitleGUI;

    public void Awake()
    {
        HighScoreGUI.SetActive(false);
        TitleGUI.SetActive(true);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HighScorePOPUP()
    {
        HighScoreGUI.SetActive(true);
        TitleGUI.SetActive(false);
        GameObject.Find("HighScoreNumber").GetComponent<UnityEngine.UI.Text>().text = ""+ PlayerPrefs.GetInt("highScore");
    }

    public void ReturnTitle()
    {
        HighScoreGUI.SetActive(false);
        TitleGUI.SetActive(true);
    }
}

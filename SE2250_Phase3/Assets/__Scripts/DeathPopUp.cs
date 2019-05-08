using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DeathPopUp : MonoBehaviour
{
    public GameObject deathMenuPOPup;
    public static bool gamePaused = false;

    private void Awake()
    {
        deathMenuPOPup.SetActive(false);
    }
    private void Update()
    {
        if(Hero.S.shieldLevel < 0)
        {
          
            deathMenuPOPup.SetActive(true);
            GameObject.Find("new2").GetComponent<UnityEngine.UI.Button>().GetComponentInChildren<Text>().text = "     Score: " + ScoreManager.scoreManager.score;
            GameObject.Find("new1").GetComponent<UnityEngine.UI.Button>().GetComponentInChildren<Text>().text = "     Highscore: " + PlayerPrefs.GetInt("highScore");
        }
    }

    public void Restart()
    {
        if (ScoreManager.scoreManager.score > PlayerPrefs.GetInt("highScore"))
        {
            ScoreManager.scoreManager.SetHighscore(ScoreManager.scoreManager.score);
        }
        ScoreManager.scoreManager.SetScore(0);
        SceneManager.LoadScene("_Scene0");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
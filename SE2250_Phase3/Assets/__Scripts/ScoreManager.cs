using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager scoreManager;

    public int score = 0;
    public int highScore;

    private void Awake()
    {
        scoreManager = this;
    }
    // Start is called before the first frame update
    private ScoreManager() { }

    public static ScoreManager Instance
    {
        get
        {
            if (scoreManager = null)
            {
                scoreManager = new ScoreManager();
            }
            return scoreManager;
        }

    }

    public void SetHighscore(int newHighScore)
    {
        PlayerPrefs.SetInt("qweasd", newHighScore);
    }
    public void UpdateScore(int enemyScore)
    {
        score += enemyScore;
        if (enemyScore == 0) score = 0;
    }
    public void UpdateScoreText()
    {
        GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "Score: " + score;
    }

    public void UpdateHighScoreText()
    {
        GameObject.Find("HighScore").GetComponent<UnityEngine.UI.Text>().text = "High Score: " + PlayerPrefs.GetInt("qweasd");
    }
}

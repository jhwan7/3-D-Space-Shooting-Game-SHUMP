using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject highScoreGUI;
    public GameObject titleGUI;
    public GameObject instructionGUI;
    public GameObject creditsGUI;
    public GameObject settingsGUI;

    public void Awake()
    {
        titleGUI.SetActive(true);
        highScoreGUI.SetActive(false);
        instructionGUI.SetActive(false);
        creditsGUI.SetActive(false);
        settingsGUI.SetActive(false);
        ScoreManager.scoreManager = gameObject.AddComponent<ScoreManager>();
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
        highScoreGUI.SetActive(true);
        titleGUI.SetActive(false);
        GameObject.Find("HighScoreNumber").GetComponent<UnityEngine.UI.Text>().text = "" + PlayerPrefs.GetInt("highScore");
    }

    public void ReturnTitle()
    {
        highScoreGUI.SetActive(false);
        instructionGUI.SetActive(false);
        titleGUI.SetActive(true);
    }

    public void LoadInstruction()
    {
        titleGUI.SetActive(false);
        instructionGUI.SetActive(true);

    }
    public void LoadCredits()
    {
        titleGUI.SetActive(false);
        creditsGUI.SetActive(true);
    }
    public void LoadSettings()
    {
        titleGUI.SetActive(false);
        settingsGUI.SetActive(true);
    }
}

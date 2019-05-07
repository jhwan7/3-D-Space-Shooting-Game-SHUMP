using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    static public Spawn S;
    //Dictionary is static so that class Spawn class instance can access and any static method of Spawn too
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    public GameObject[] prefabEnemies;
    public float enemySpawnPeriod = 0.5f;
    public float enemyDefaultPadding = 1.5f;


    public Slider x2Slider;
    public float timeTracker = 0f;

    //Instantiating an array that holds the different weapons
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.spread, WeaponType.shield, WeaponType.nuke, WeaponType.EMP, WeaponType.X2
    };

    public GameObject nukeEffect;
    public GameObject empEffect;

    private BoundsCheck _bndCheck;

    //Fields related to keeping the score


    //Keep track of the number of nukes, start at 0
    public int nukeCounter;

    //Fields related to score and level
    public int currentLevel = 1;
    public GameObject levelDisplay;
    public Text levelText;
    public float scoreBase = 1000f;
    public float scoreNextLevel;
    public bool isNewLevel;

    //Fields related to double points
    public bool isDoubleTime;
    public float runningTime;
    public float pickupTime;
    public GameObject doubleTimeText;
    private float levelTimeStart;

    private void Awake()
    {
        if (Time.time < 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        levelDisplay.SetActive(true);
        doubleTimeText.SetActive(false);
        isDoubleTime = false;
        pickupTime = 0f;
        Time.timeScale = 1;
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        //A generic dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def; //In the dictionary we are attaching the specifications (value) of each weapon to the weapon name(key)
        }

        ScoreManager.scoreManager.UpdateScoreText();
        GameObject.Find("NukeCounter").GetComponent<UnityEngine.UI.Text>().text = "Nuke Counter: " + nukeCounter;

        // Check for a high score in PlayerPrefs
        if (PlayerPrefs.HasKey("highScore"))
        {
            //GameObject.Find("HighScore").GetComponent<UnityEngine.UI.Text>().text = "High Score: " + ScoreManager.scoreManager.highScore;
            ScoreManager.scoreManager.UpdateHighScoreText();
        }
        else
        {
            ScoreManager.scoreManager.SetHighscore(0);
        }
    }

    private void Update()
    {
        SetLevel();
        if (levelDisplay.activeSelf)
        {
            if (Time.time - levelTimeStart > 1f)
            {
                levelDisplay.SetActive(false);
                Invoke("SpawnEnemy", 1f / enemySpawnPeriod);
            }
        }

        runningTime = Time.time;

        //After 10 seconds, turn off double points points
        if (runningTime - pickupTime > 10)
        {
            isDoubleTime = false;
            doubleTimeText.SetActive(false);
        }

        if (isDoubleTime)
        {

            doubleTimeText.SetActive(true);
            //Display countdown of 10 seconds, which is time limit of the double points 
            GameObject.Find("Double").GetComponent<UnityEngine.UI.Text>().text = "Double Time: " + (int)(pickupTime + 10 - runningTime);

        }

        if (isDoubleTime)
        {
            if (runningTime - pickupTime > timeTracker)
            {
                x2Slider.value = x2Slider.value - 0.002f;
                timeTracker += 0.02f;
            }
        }

    }

    public void SpawnEnemy()
    {
        if (!levelDisplay.activeSelf)
        {
            int index = Random.Range(0, prefabEnemies.Length);
            GameObject go = Instantiate<GameObject>(prefabEnemies[index]);


            float enemyPadding = enemyDefaultPadding;

            if (go.GetComponent<BoundsCheck>() != null)
            {
                enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
            }

            Vector3 pos = Vector3.zero;
            float xMin = -_bndCheck.camWidth + enemyPadding;
            float xMax = _bndCheck.camWidth - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = _bndCheck.camHeight + enemyPadding;
            go.transform.position = pos;

            Invoke("SpawnEnemy", 1f / enemySpawnPeriod);
        }
    }

    public void SetLevel()
    {
        isNewLevel = false;
        scoreNextLevel = Mathf.Pow(currentLevel, 1.5f) * scoreBase;
        if (ScoreManager.scoreManager.score >= scoreNextLevel)
        {
            currentLevel++;
            isNewLevel = true;
            removeEnemies();
            levelText.text = "Level: " + currentLevel;
            levelDisplay.SetActive(true);
            levelTimeStart = Time.time;
            //Decrease the time in which enemies spawn, as levels increase the spawn time decreases
            enemySpawnPeriod += 0.025f * currentLevel;
        }
    }

    public void removeEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    /* DelayedRestart and Restart are called in Hero class when the player dies
     */
    public void DelayedRestart(float delay)
    {
        //Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    //Restart() is called in DelayedRestart() method
    public void Restart()
    {
        if (ScoreManager.scoreManager.score > PlayerPrefs.GetInt("qweasd"))
        {
            ScoreManager.scoreManager.SetHighscore(ScoreManager.scoreManager.score);
            ScoreManager.scoreManager.UpdateScore(0);
        }

        //Reload _Scene_0 to restart the game
        SceneManager.LoadScene("_Scene0");
    }

    /* Static function that gets a WeaponDefinition from the WEAP_DICT static
     * protected field of the Spawn class.
     * The WeaponDefinition or, if there is no WeaponDefinition with the WeaponType
     * passed in, returns a new WeaponDefinition with a WeaponType of none
     */
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //Check to make sure that the key exists in the Dictionary
        //Attempting to retrieve a key that didn't exist, would throw an error
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }

        //This returns a new WeaponDefinition with a type of WeaponType.non
        //which means it has failed to find the right WeaponDefinition.
        return (new WeaponDefinition());
    }

    public void shipDestroyed(Enemy e)
    { // c
      // Potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance)
        { // 
          // Choose which PowerUp to pick
          // Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length); // 
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType); // 
                                // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }

    
}

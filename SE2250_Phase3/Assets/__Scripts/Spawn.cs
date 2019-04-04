using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour
{
    static public Spawn S;
    //Dictionary is static so that class Spawn class instance can access and any static method of Spawn too
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT; 

    public GameObject[] prefabEnemies;
    public float enemySpawnPeriod = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    //Instantiating an array that holds the different weapons
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield, WeaponType.nuke, WeaponType.EMP
    };

    public GameObject nukeEffect;
    public GameObject empEffect;

    private BoundsCheck _bndCheck;

    //Fields related to keeping the score
    public int score = 0;
    public int highScore;

    //Keep track of the number of nukes
    public int nukeCounter = 0;

    public int currentLevel = 1;

    private void Awake()
    {
        Time.timeScale = 1;
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPeriod);

        //A generic dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def; //In the dictionary we are attaching the specifications (value) of each weapon to the weapon name(key)
        }

        GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "Score: " + score;
        GameObject.Find("NukeCounter").GetComponent<UnityEngine.UI.Text>().text = "Nuke Counter: " + nukeCounter;

        // Check for a high score in PlayerPrefs
        if (PlayerPrefs.HasKey("highScore"))
        {
            GameObject.Find("HighScore").GetComponent<UnityEngine.UI.Text>().text = "High Score: " + PlayerPrefs.GetInt("highScore");
        }
        else
        {
            PlayerPrefs.SetInt("highScore", 0);
        }
    }

    public void SpawnEnemy()
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

    /* DelayedRestart and Restart are called in Hero class when the player dies
     */   
    public void DelayedRestart( float delay)
    {
        //Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    //Restart() is called in DelayedRestart() method
    public void Restart()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
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
        { // d
          // Choose which PowerUp to pick
          // Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length); // e
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType); // f
                                // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour
{
    static public Spawn S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT; //Dictionary is static so that class Spawn class instance can access and any static method of Spawn too

    public GameObject[] prefabEnemies;
    public float enemySpawnPeriod = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck _bndCheck;

    private void Awake()
    {
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPeriod);

        //A generic dictionary with WeaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def; //In the dictionary we are attaching the specifications (value) of each weapon to the weapon name(key)
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

    public void DelayedRestart( float delay)
    {
        //Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        //Reload _Scene_0 to restart the game
        SceneManager.LoadScene("_Scene0");
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        return (new WeaponDefinition());
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }
}

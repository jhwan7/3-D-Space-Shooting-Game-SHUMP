using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    static public Spawn S;

    public GameObject[] prefabEnemies;
    public float enemySpawnPeriod = 2f;
    public float enemyDefaultPadding = 1.5f;

    private BoundsCheck _bndCheck;

    private void Awake()
    {
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPeriod);

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

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

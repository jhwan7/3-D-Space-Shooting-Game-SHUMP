using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Summary of the possible weapons we can use in the game
public enum WeaponType
{
    none,
    blaster,
    spread,
    phaser,
    missile,
    laser,
    shield
}

[System.Serializable] //Causes the class defined to be serializable and editable within the Unity inspector
public class WeaponDefinition 
{
    public WeaponType type = WeaponType.none;
    public string letter; 
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float continuousDamage = 0;
    public float delayBetweenShots = 0;
    public float velocity = 20;
}

public class Weapons : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

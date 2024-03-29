﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;
    [Header("Set in Inspector")]
    public float speed = 30f;
    public float rollMult = -45f;
    public float pitchMult = 30f;

    //gameRestartDelay is related to the time delay between restarting the game
    public float gameRestartDelay = 2f;

    //Adding shoot-ability 
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

    //Shield status
    [Header("Set Dynamically")]
    [SerializeField] //Allows us to see the private variable in the inspector
    private float _shieldLevel = 4;
    //This variable holds a reference to the last triggering Gameobject
    private GameObject _lastTriggerGo = null;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    private void Awake()
    {
        if(S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Aware() - Attempted to assign second Hero.S!");
        }

        //fireDelegate += TempFire;
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //Allow the ship to fire
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null) //Jump is equivalent to 1 when spacebar is pressed
        {
            fireDelegate();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        //Transform.root gives you the transform of the root GameObject
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        //Make sure it's not the same triggering go as last time
        if (go == _lastTriggerGo)
        {
            return;
        }
        _lastTriggerGo = go;

        /* If the shield was triggered by an enemy decrease the the level
         * of the shield by 1 and destroy the enemy
         */

        if(go.tag == "Enemy") 
        {
            shieldLevel--;
            Destroy(go); 
            print("Trigger");
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel); //Returns the value of the _shieldLevel
        }
        set
        {
            /*Ensure the shield level is never set to a number higher than 4
             * and when the shield is destroyed 4 times, the player loses
             * and the game restarts
             */            

            _shieldLevel = Mathf.Min(value, 4); //ensures that _shieldLevel is never set to a number higher than 4
            //If shield is going to be set to less than zero
            if (value < 0) //If the value passed into the set is less than 0, _Hero is destroyed
            {
                Destroy(this.gameObject);
                Spawn.S.DelayedRestart(gameRestartDelay); //This line restarts the game when all the player's shields are destroyed
            }
        }
    }
}

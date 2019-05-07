using System.Collections;
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
    public Weapons[] weapons;

    public GameObject playerExplosion;

    //Shield status
    [Header("Set Dynamically")]
    [SerializeField] //Allows us to see the private variable in the inspector
    private float _shieldLevel = 4;
    //This variable holds a reference to the last triggering Gameobject
    private GameObject _lastTriggerGo = null;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    private void Start()
    {
        if(S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Aware() - Attempted to assign second Hero.S!");
        }

        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
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
            Enemy enemy0 = (Enemy)go.GetComponent("Enemy");
            shieldLevel--;
            enemy0.Explode();
            Destroy(go); 
            print("Trigger");
        }
        else if (go.tag == "PowerUp")
        {
            // If the shield was triggered by a PowerUp
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.nuke:
                //When the nuke is absorbed by the user, increment the number of nukes allowed
                Spawn.S.nukeCounter++;
                //Update the UI-text
                GameObject.Find("NukeCounter").GetComponent<UnityEngine.UI.Text>().text = "Nuke Counter: " + Spawn.S.nukeCounter;
                break;
            case WeaponType.shield: // a
                shieldLevel++;
                break;

            case WeaponType.EMP:
                //Call stun method
                pu.Stun();
                break;

            case WeaponType.X2:
                Spawn.S.pickupTime = Time.time;
                Spawn.S.isDoubleTime = true;
                Spawn.S.x2Slider.value = 1;
                Spawn.S.timeTracker = 0;
                //Spawn.S.InitiateDoublePoints();
                break;

            default:
                if(pu.type == weapons[0].type)
                {
                    Weapons w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    Weapons GetEmptyWeaponSlot()
    {
        for (int i =0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }

    void ClearWeapons()
    {
        foreach (Weapons w in weapons)
        {
            w.SetType(WeaponType.none);
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
                Instantiate(playerExplosion, transform.position, transform.rotation);
                Destroy(this.gameObject);
                //Spawn.S.DelayedRestart(gameRestartDelay); //This line restarts the game when all the player's shields are destroyed
            }
        }
    }

}

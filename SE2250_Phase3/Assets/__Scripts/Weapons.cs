using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Summary of the possible weapons we can use in the game
/* As a public enum outside of the Weapon class, WeaponType cdan be seen by and use by any other script in the project
 */
public enum WeaponType
{
    none, //Default, no weapon
    blaster, //Simple blaster
    spread, //3 shots
    phaser, 
    missile,
    laser,
    shield,
    nuke,
    EMP,
    X2
}

/* the WeaponDefinition class allows you to set the properties
 * of a specific weapon in the Inspector. The Spawn class has
 * an array of WeaponDefinitions that makes this possible
 */

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
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        //Call SetType() for the default _type of WeaponType.none
        SetType(_type);

        //Dynamically create an anchor for all Projectiles
        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        //Find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject;
        if(rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Spawn.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0; //you can fire immediately after _type is set
    }

    public void Fire()
    {
        // If this.gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return; 
        // If it hasn't been enough time between shots, return

        if (Time.time - lastShotTime < def.delayBetweenShots)
        { 
            return;
        }

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity; // j

        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.spread: // l
                p = MakeProjectile(); // Make middle Projectile
                p.rigid.velocity = vel;
                p = MakeProjectile(); // Make right Projectile
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile(); // Make left Projectile
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }

    void Nuke()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            Enemy enemy0 = (Enemy)enemy.GetComponent("Enemy");
            if(Spawn.S.isDoubleTime)
            {
                Spawn.S.score += enemy0.score*2;
            }
            else
            {
                Spawn.S.score += enemy0.score;
            }
            GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "Score: " + Spawn.S.score;
            enemy0.Explode();
            Destroy(enemy);
        }
        Instantiate(Spawn.S.nukeEffect);
        Spawn.S.nukeCounter--;
        GameObject.Find("NukeCounter").GetComponent<UnityEngine.UI.Text>().text = "Nuke Counter: " + Spawn.S.nukeCounter;
    }


    public Projectile MakeProjectile()
    { 
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);

        if (transform.parent.gameObject.tag == "Hero")
        { 
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true); 
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time; 
        return (p);
    }

    public void Stun()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy e = (Enemy)enemy.GetComponent("Enemy");
            e.stun = true;
            Instantiate(Spawn.S.empEffect,e.transform.position,e.transform.rotation);
        }
    }

    void Update()
    {
        /* Pressing 'E' allows the user to switch between blaster and spread
         */
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_type == WeaponType.spread)
            {
                _type = WeaponType.blaster;
                SetType(_type);
            }
            else
            {
                _type = WeaponType.spread;
                SetType(_type);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Spawn.S.nukeCounter > 0)
            {
                Nuke();
            }
            else
            {
                return;
            }

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10f;
    public int score = 0;

    public bool notifiedOfDestruction = false;
    public float powerUpDropChance = 1f;

    protected BoundsCheck bndCheck;//using bounds check class

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>(); //checks to see if BoundsCheck script is attached, if not bndCheck set to NULL
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (bndCheck != null && bndCheck.offDown)//if script is attached and bndCheck 
        {
            Destroy(gameObject);
        }
        

    }

    public virtual void Move()
    {
        Vector3 tempPos = pos; //gets the current position of object
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                //If this enemey is off screen, don't damage it
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                //Hurt the enemy
                //Get the damage amount from the Spawn WEAP_DICT
                health -= Spawn.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    if (!notifiedOfDestruction)
                    {
                        Spawn.S.shipDestroyed(this);
                    }
                    notifiedOfDestruction = true;

                    Spawn.S.score = Spawn.S.score + score;
                    GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "Score: " + Spawn.S.score;

                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;
    public float health = 10f;
    public int score = 2;
    public float powerUpDropChance = 0.3f;

    private bool _notifiedOfDestruction = false;
    public GameObject explosionEffect;

    private bool _stunned = false;

    public bool stun
    {
        get
        {
            return (this._stunned);
        }
        set
        {
            this._stunned = value;
        }
    }

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
        if(stun == false)
        {
            Vector3 tempPos = pos; //gets the current position of object
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
        else
        {
            return;
        }
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
                    if (!_notifiedOfDestruction)
                    {
                        Spawn.S.shipDestroyed(this);
                    }
                    _notifiedOfDestruction = true;

                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;

            case "Hero":
                Explode();
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }

    }

    private void OnDestroy()
    {
        //If enemies come off of screen(out of bounds), they will not explod
        if (bndCheck != null && bndCheck.offDown)
        {
            return;
        }

        else if(Spawn.S.isNewLevel)
        {
            return;
        }

        if (_notifiedOfDestruction)
        {
            //If enemies are destroyed, increase the score and make them explode
            if(Spawn.S.isDoubleTime)
            {
                Debug.Log("x2 score on");
                ScoreManager.scoreManager.UpdateScore(score * 2)  ;

            }
            else
            {
                ScoreManager.scoreManager.UpdateScore(this.score);
            }
            GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "Score: " + ScoreManager.scoreManager.score;
            Explode();

        }
      
    }

    /*
     * This method will instantiate a explosion VFX (with sound) on the location that the enemy is destroyed
     */

    public void Explode()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        else
        {
            //Instantiate explosion VFX on the location of where the enemy is destroyed
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
    }
}


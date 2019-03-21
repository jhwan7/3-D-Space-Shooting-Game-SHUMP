using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float speed = 10f;
    //public float fireRate = 0.3f;
    //public float health = 10f;
    //public int score = 100;

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
        GameObject otherGO = coll.gameObject; //Get the GameObject of the collider that was hit in the collision
        if(otherGO.tag == "ProjectileHero") { //When the projectileHero hits the enemy, we destroy both objects
            Destroy(otherGO);
            Destroy(gameObject);
            Debug.Log("Hit");
        }
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }

        //GameObject otherGo = coll.gameObject;
        //switch (otherGo.tag)
        //{
        //    case "ProjectileHero":

        //}
    }
    //void OnCollisionEnter(Collision coll)
    //{
    //    GameObject otherGO = coll.gameObject;

    //    switch (otherGO.tag)
    //    {
    //        case "ProjectileHero":
    //            Projectile p = otherGO.GetComponent<Projectile>();
    //            if (!bndCheck.isOnScreen)
    //            {
    //                Destroy(otherGO);
    //                break;
    //            }
    //            //health -= Main.GetWeaponDefinition(p.type).damageOnHit;
    //            //if (health <= 0)
    //            //{
    //            //    Destroy(this.gameObject);
    //            //}
    //            Destroy(otherGO);
    //            break;

    //        default:
    //            print("Enemy hit by non-ProjectileHero: " + otherGO.name);
    //            break;
    //    }

    //}
}


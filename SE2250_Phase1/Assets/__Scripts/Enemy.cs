using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10f;
    public int score = 100;

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
}


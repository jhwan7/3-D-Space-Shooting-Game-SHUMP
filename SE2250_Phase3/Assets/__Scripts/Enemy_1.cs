using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    int i;
   
    void Start()
    {
        i = Random.Range(0, 15);
    }

    public override void Move()
    {
        Vector3 tempPos = pos; //gets the current position of object
              
        if (i % 2 == 0 && stun == false)
        {
            tempPos.y -= speed * Time.deltaTime;
            tempPos.x -= speed * Time.deltaTime;
            pos = tempPos;

        }
        if (i % 2 == 1 && stun == false)
        {
            tempPos.y -= speed * Time.deltaTime;
            tempPos.x += speed * Time.deltaTime;
            pos = tempPos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject teleportIndicator;
    bool teleportDecision = false;
    Vector3 teleportPosition;
    bool once = false;
    GameObject teleport;
    
    // Start is called before the first frame update
    void Start()
    {
        //teleportDecision = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        teleportPosition = Hero.S.transform.position;
        
        if (Input.GetKeyDown(KeyCode.T) )
        { 
            if(teleportDecision == false && once == false)
            {
                teleportDecision = true;
                teleport = Instantiate(teleportIndicator, teleportPosition, Quaternion.Euler(0, 0, 0));
            }
         
            if (teleportDecision == true && once == true)
                    {
                        Hero.S.transform.position = teleport.transform.position;
                        Destroy(teleport);
                        //list1.RemoveAt(0);
                        teleportDecision = false;
                    }
            once = (once ==  false) ? true : false;
        }
        

    }
}

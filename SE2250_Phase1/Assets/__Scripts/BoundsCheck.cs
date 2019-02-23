using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps GameObject on Screen
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    
    public float radius = 1f; //set to -2.5 for enemies as it allows them to go off the screen before we destroy them
    public bool keepOnScreen = true; //set to false for enemies as we don't force them to stay on screen, true for hero 

    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    public bool offRight, offLeft, offUp, offDown;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if(pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            isOnScreen = false;
            offRight = true;
        }

        if(pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            isOnScreen = false;
            offLeft = true;
        }
        if(pos.y >camHeight - radius)
        {
            pos.y = camHeight - radius;
            isOnScreen = false;
            offUp = true;
        }
        if(pos.y<-camHeight + radius)
        {
            pos.y = -camHeight + radius;
            isOnScreen = false;
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);
        //if object is one that should be on screen and has exited the screen then we alter position
        if(keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

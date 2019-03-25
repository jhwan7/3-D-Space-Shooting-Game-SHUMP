using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck _bndCheck;
    private Renderer _rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;

    //This public property masks the field _type and takes action when it is set
    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); } 
    }


    private void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
        _rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    /* Sets the _type private field and colors this projectile to match
     * the WeaponDefinition
     */

    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Spawn.GetWeaponDefinition(_type);
        _rend.material.color = def.projectileColor;
    }

    void Update()
    {
        //If the projectile leaves the screen, we will destroy it
        if (_bndCheck.offUp) 
        {
            Destroy(gameObject);
        }
    }
}

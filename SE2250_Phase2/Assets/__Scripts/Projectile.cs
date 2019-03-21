using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck _bndCheck;
    private Renderer _rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField] //Allows Weapon type to be visible in the inspector, even though it's private
    private WeaponType _type;

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

    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Spawn.GetWeaponDefinition(_type);
        _rend.material.color = def.projectileColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bndCheck.offUp) //If the projectile leaves the screen, we will destroy it
        {
            Destroy(gameObject);
        }
    }
}

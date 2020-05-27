using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springer_WeaponManager : MonoBehaviour
{


    [SerializeField]
    Weapon[] AvailableWeapons;

    [SerializeField]
    GameObject MeshObject;
    [SerializeField]
    GameObject ProjectilePosition;

    MeshRenderer MR;
    MeshFilter MF;

    [SerializeField]
    GameObject Projectile;

    int WeaponIndex = 0;

    int DirectionModifier = 1;

    
    
   protected int ActiveWeapon;

    int CurrentCapacity;
    int MaximumCapacity;
    bool bCanFire = true;


    private void Start()
    {
        MR = MeshObject.GetComponent<MeshRenderer>();
        MF = MeshObject.GetComponent<MeshFilter>();
        SetWeapon(0);
    }

    private void Awake()
    {
       
    }


     public void FireWeapon()
    {
        //gameObject ProjectileRef = Instantiate(Projectile)
        //GetTargetPosition();
        GameObject Proj = Instantiate(Projectile, ProjectilePosition.transform.position, ProjectilePosition.transform.rotation);
        Proj.GetComponent<ProjectileBase>().SetDirection(DirectionModifier);
    }

    public void EquipWeapon()
    {

    }

    public void DropWeapon()
    {

    }

    void SwitchWeapon(int Direction)
    {
        ActiveWeapon += ((Direction + ActiveWeapon) % AvailableWeapons.Length);
         MF.mesh = AvailableWeapons[ActiveWeapon].WeaponMesh;
        Projectile = AvailableWeapons[ActiveWeapon].Projectile;




    }

    void SetWeapon(int Index)
    {
        MF.mesh = AvailableWeapons[ActiveWeapon].WeaponMesh;
        Projectile = AvailableWeapons[ActiveWeapon].Projectile;
    }

   public void SetDirectionModifier(int Dir)
    {
        if (Dir > 0) DirectionModifier = 1;
        else if (Dir < 0) DirectionModifier = -1;
    }


    
}

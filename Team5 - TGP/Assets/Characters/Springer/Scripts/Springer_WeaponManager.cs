﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springer_WeaponManager : MonoBehaviour
{


    [SerializeField]
    List<Weapon> AvailableWeapons = new List<Weapon>();

    [SerializeField]
    GameObject MeshObject;
    [SerializeField]
    GameObject ProjectilePosition;

    MeshRenderer MR;
    MeshFilter MF;

    [SerializeField]
    GameObject Projectile;

    [SerializeField]
    private AudioClip FireSFX;

    int WeaponIndex = 0;

    int DirectionModifier = 1;

    EWeaponType WeaponMode;


    int FireCost = 1;

    float BurstRate = 0.3f;
    float FireRate;

    float WeaponTimer;



    int BurstSize = 3;
    int ActiveBurstSize = 3;
    
   protected int ActiveWeapon;



    bool PrevFireInput;

    int CurrentCapacity = 100;
    int MaximumCapacity = 100;
    bool bCanFire = true;


    private void Start()
    {
       
        SetWeapon(0);
    }

    private void Awake()
    {
        MR = MeshObject.GetComponent<MeshRenderer>();
        MF = MeshObject.GetComponent<MeshFilter>();
        if (AvailableWeapons.Count > 0)
        {
            SetWeapon(0);
        }
    }

    private void Update()
    {

        if (!bCanFire) WeaponTimer -= Time.deltaTime;

        switch (WeaponMode)
        {
           
            case EWeaponType.Burst:
                if (!bCanFire)
                {
                    if (ActiveBurstSize > 0 && WeaponTimer <= 0)
                    {
                        WeaponTimer = BurstRate;
                        ActiveBurstSize--;
                        Create_SingleShot();
                    }
                    else if (WeaponTimer <= 0)
                    {
                        WeaponTimer = FireRate;
                        bCanFire = true;
                        ActiveBurstSize = BurstSize;
                    }
                }


                break;
            case EWeaponType.RapidFire:
                if (WeaponTimer <= 0.0f && IsCapacityValid())
                {
                    bCanFire = true;
                }

                break;
            case EWeaponType.Single:
                if (WeaponTimer <= 0.0f && PrevFireInput == false && IsCapacityValid())
                {
                    bCanFire = true;
                    WeaponTimer = FireRate;
                }
                break;
        }
    }


    public void FireWeapon(bool Input)
    {
        //gameObject ProjectileRef = Instantiate(Projectile)
        //GetTargetPosition();

        
        

        if (bCanFire && Input)
        {
            AudioManager.Instance().PlaySFXPlayer(FireSFX);

            switch (WeaponMode)
            {
                case EWeaponType.Beam:
                    
                    break;
                case EWeaponType.Burst:
                   // CurrentCapacity -= FireCost;
                    WeaponTimer = 0.0f;
                    bCanFire = false;
                    break;
                case EWeaponType.RapidFire:
                    Create_SingleShot();
                   // CurrentCapacity -= FireCost;
                    bCanFire = false;
                    WeaponTimer = FireRate;
                    break;
                case EWeaponType.Single:
                    //CurrentCapacity -= FireCost;
                    Create_SingleShot();
                    bCanFire = false;
                   
                    break;
            }
        }


       
       

        PrevFireInput = Input;
    }

    void Create_SingleShot()
    {
        GameObject Proj = Instantiate(Projectile, ProjectilePosition.transform.position, ProjectilePosition.transform.rotation);
        Proj.GetComponent<ProjectileBase>().SetDirection(DirectionModifier);

    }
 
    void Create_BeamShot()
    {

    }

    public void EquipWeapon(Weapon NewWeapon)
    {
       if (!HasWeapon(NewWeapon))
        {
            AvailableWeapons.Add(NewWeapon);
        }
    }

    public void DropWeapon()
    {

    }

  public  void SwitchWeapon(int Direction)
    {
        
       


        if ((ActiveWeapon + Direction) >= AvailableWeapons.Count)
        {
            ActiveWeapon = 0;
        }
        else if ((ActiveWeapon + Direction) < 0)
        {
            ActiveWeapon = AvailableWeapons.Count - 1;
        }
        else
        {
            ActiveWeapon += Direction;
        }

        

        SetWeapon(ActiveWeapon);


    }

    void SetWeapon(int Index)
    {
        Debug.Log("Set Weapon: " + "(" + ActiveWeapon + "), Weapon Name: " + AvailableWeapons[ActiveWeapon].WeaponName);

        ActiveWeapon = Index;
        MF.mesh = AvailableWeapons[ActiveWeapon].WeaponMesh;
        Projectile = AvailableWeapons[ActiveWeapon].Projectile;
        WeaponMode = AvailableWeapons[ActiveWeapon].WeaponType;
        FireRate = AvailableWeapons[ActiveWeapon].FireRate;
        MaximumCapacity = AvailableWeapons[ActiveWeapon].Capacity;
    }

   public void SetDirectionModifier(int Dir)
    {
        if (Dir > 0) DirectionModifier = 1;
        else if (Dir < 0) DirectionModifier = -1;
    }

    bool IsCapacityValid()
    {
        return !(CurrentCapacity <= 0);
    }

    bool HasWeapon(Weapon Item)
    {
        for (int i = 0; i < AvailableWeapons.Count; i++)
        {
            if (AvailableWeapons[i] == Item) return true;

        }
        return false;
    }
    
}

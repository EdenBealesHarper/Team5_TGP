using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType {Single, Burst, RapidFire, Beam };

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons")]
public class Weapon : ScriptableObject
{

   public GameObject Projectile;
    public Mesh WeaponMesh;
   public string WeaponName;
   public EWeaponType WeaponType;

    public int Capacity;
   public float FireRate;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon heldWeapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Yeet");

        collision.GetComponent<Springer_WeaponManager>().EquipWeapon(heldWeapon);

        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon heldWeapon;
    public AudioClip sfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Springer_WeaponManager>().EquipWeapon(heldWeapon);
        AudioManager.Instance().PlaySFXWorld(sfx);

        Destroy(this.gameObject);
    }
}

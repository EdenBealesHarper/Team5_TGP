using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon heldWeapon;
    public AudioClip sfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Springer_WeaponManager>().EquipWeapon(heldWeapon);
            AudioManager.Instance().PlaySFXWorld(sfx);

            Destroy(this.gameObject);
        }
    }
}

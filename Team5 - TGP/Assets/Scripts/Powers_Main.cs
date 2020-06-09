using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Powers_Main : MonoBehaviour
{
    public List<string> slottedUpgrades = new List<string>();
    public List<string> knownUpgrades = new List<string>();

    [SerializeField]
    private float ChargedJumpForce;             //Maximum jump force for charged jump.
    [SerializeField]
    private float ChargedJumpIncrement;         //How much the jump gets charged per frame.
    [SerializeField]
    private float ChargedJumpMovementPenalty;   //how much the player movespeed is reduced when charging the jump.


    
    private float InitialJumpForce;             //Gets the initial jump force that the player is given.
    private float InitialMaxMoveSpeed;             //Get the intial max move speed the player is given.

    public int fireTime = 0;
    public int fireMax = 1000;

    public bool onFire = false;

    private Springer_CharacterController CharControl;
    private Springer_WeaponManager WeaponManager;

    public bool dirty;

    [SerializeField]
    private AudioClip pickupSuccessSFX;
    [SerializeField]
    private AudioClip pickupFailSFX;
    [SerializeField]
    private AudioClip chargeJumpSFX;

    void Start()
    {
        CharControl = GetComponent<Springer_CharacterController>();
        InitialJumpForce = CharControl.JumpForce;
        InitialMaxMoveSpeed = CharControl.MaxSpeed;
        dirty = false;
    }

    void Update()
    {
        foreach (string upgrade in slottedUpgrades)
        {
            switch (upgrade)
            {
                case("ENGINE"):
                    {
                        boostEngine();
                        break;
                    }
                case ("CHARGEJUMP"):
                    {
                        chargeJump();
                        break;
                    }
                case ("DOUBLEJUMP"):
                    {
                        doubleJump();
                        break;
                    }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "UPGRADE")
        {
            if (!knownUpgrades.Contains(other.gameObject.name))
            {
                knownUpgrades.Add(other.gameObject.name);
                Destroy(other.gameObject);

                slotUpgrade(other.gameObject.name);
            }
        }
    }

    public void slotUpgrade(string upgradeName)
    {
        if (knownUpgrades.Contains(upgradeName))
        {
            if (!slottedUpgrades.Contains(upgradeName))
            {
                slottedUpgrades.Add(upgradeName);
                AudioManager.Instance().PlaySFXWorld(pickupSuccessSFX);
                dirty = true;
            }
            else
            {
                print("Upgrade Already Slotted");
                AudioManager.Instance().PlaySFXWorld(pickupFailSFX);
            }
        }
        else
        {
            print("Sorry, you don't know that upgrade");
            AudioManager.Instance().PlaySFXWorld(pickupFailSFX);
        }
    }

    public void boostEngine()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            fireTime++;

            CharControl.SpeedModifier = 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            CharControl.SpeedModifier = 1.0f;
        }
        else
        {
            if (fireTime > 0)
            {
                fireTime--;
            }
        }

        if (fireTime >= fireMax)
        {
            //Deal Damage and set on fire
            GetComponent<Health>().TakeDamage(0.1f);
            onFire = true;
        }
        else 
        {
            onFire = false;
        }
    }

    public void chargeJump()
    {
        AudioManager.Instance().PlaySFXPlayer(chargeJumpSFX);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            print("Charging Jump");

            //While the key is held, charges the jump force up until it's maximum.

            if (CharControl.JumpForce <= ChargedJumpForce)
            {
                CharControl.JumpForce += ChargedJumpIncrement;
                //Reduces the movement speed of the player while charging.
                CharControl.MaxSpeed = InitialMaxMoveSpeed * ChargedJumpMovementPenalty;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            print("Release");


            //Returns JumpForce and Movespeed to normal values.
            CharControl.JumpForce = InitialJumpForce;
            CharControl.MaxSpeed = InitialMaxMoveSpeed;
        }
    }

    public void doubleJump()
    {
        CharControl.JumpCount = 2;
    }
}


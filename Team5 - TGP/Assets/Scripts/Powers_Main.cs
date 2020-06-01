using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Powers_Main : MonoBehaviour
{
    public List<string> slottedUpgrades = new List<string>();
    public List<string> knownUpgrades = new List<string>();

    public int fireTime = 0;
    public int fireMax = 1000;

    public bool onFire = false;

    private Springer_CharacterController CharControl;

    public bool dirty;

    void Start()
    {
        CharControl = GetComponent<Springer_CharacterController>();
        dirty = false;
    }

    void Update()
    {
        foreach (string upgrade in slottedUpgrades)
        {
            if (upgrade == "ENGINE")
            {
                boostEngine();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Hmm Colliding?");

        if(other.gameObject.tag == "UPGRADE")
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
                dirty = true;
            }
            else
            {
                print("Upgrade Already Slotted");
            }
        }
        else
        {
            print("Sorry, you don't know that upgrade");
        }
    }

    public void boostEngine()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            print("Zoomin");

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
            // GetComponent<Health>().TakeDamage(Time.deltaTime);
            onFire = true;
        }
        else 
        {
            onFire = false;
        }
    }
}


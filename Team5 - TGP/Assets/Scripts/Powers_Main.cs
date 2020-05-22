using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Powers_Main : MonoBehaviour
{
    public List<string> slottedUpgrades = new List<string>();
    public List<string> knownUpgrades = new List<string>();

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
            if (upgrade == "PU_ENGINE")
            {
                boostEngine();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Hmm Colliding?");

        if (other.gameObject.name.StartsWith("PU_"))
        {
            if (!knownUpgrades.Contains(other.gameObject.name))
            {
                knownUpgrades.Add(other.gameObject.name);
                Destroy(other.gameObject);

                slotUpgrade(other.gameObject.name);
            }
        }
    }

    public void boostEngine()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //LERP speed up to value
            //Increase fire counter

            //Add LERP speed back to normal when release

            CharControl.SpeedModifier = 10.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            CharControl.SpeedModifier = 1.0f;
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
}


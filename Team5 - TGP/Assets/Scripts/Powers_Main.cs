using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Powers_Main : MonoBehaviour
{
    public List<string> slottedUpgrades = new List<string>();
    public List<string> knownUpgrades = new List<string>();

    void Start()
    {

    }

    void Update()
    {
        foreach (string upgrade in slottedUpgrades)
        {
            if (upgrade == "SPEED20")
            {
                print("SPEED20");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("PU_"))
        {
            string nameNoPU = other.gameObject.name;
            nameNoPU.Replace("PU_", string.Empty);
            knownUpgrades.Add(nameNoPU);
        }
    }

    public void slotUpgrade(string upgradeName)
    {
        if (!slottedUpgrades.Contains(upgradeName))
        {
            slottedUpgrades.Add(upgradeName);
        }
        else 
        {
            print("Upgrade Already Slotted");
        }
    }
}


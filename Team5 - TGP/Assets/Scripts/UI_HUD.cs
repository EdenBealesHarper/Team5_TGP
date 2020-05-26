using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUD : MonoBehaviour
{
    private Health health;
    private float previousHealth;

    [SerializeField]
    private Image healthBar;

    private Sprite[] healthFill = new Sprite[3];

    private Powers_Main powers;
    private float previousFireTime;

    [SerializeField]
    private GameObject[] powersDisplay; // todo make dynamic if necessary
    [SerializeField]
    private GameObject fireFill;

    private Sprite[] powerSprites = new Sprite[2]; //todo get actual images

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            health = player.GetComponent<Health>();
            powers = player.GetComponent<Powers_Main>();

            healthFill[0] = Resources.Load<Sprite>("UI/healthFull");
            healthFill[1] = Resources.Load<Sprite>("UI/healthHalf");
            healthFill[2] = Resources.Load<Sprite>("UI/healthTwenty");

            powerSprites[0] = Resources.Load<Sprite>("UI/blank");
            powerSprites[1] = Resources.Load<Sprite>("UI/occupied");

            previousHealth = 0f;
            previousFireTime = powers.fireMax;
        }
        else Debug.Log("Player character not found");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        UpdatePowers();
        UpdateFire();
    }

    private void UpdateHealth()
    {
        float percentageHealth = health.GetCurrentHealth() / health.GetTotalHealth();

        // only update UI if health has changed from last frame

        if (previousHealth != percentageHealth)
        {
            // health bar changes colour from 100-50%, 50-20%, 20-0%
            // only change image if necessary

            if (percentageHealth <= 0.2f && previousHealth > 0.2f)
            {
                healthBar.sprite = healthFill[2];
            }
            else if (percentageHealth > 0.2f && percentageHealth <= 0.5f && (previousHealth <= 0.2f || previousHealth > 0.5f))
            {
                healthBar.sprite = healthFill[1];
            }
            else if (percentageHealth > 0.5f && previousHealth <= 0.5f)
            {
                healthBar.sprite = healthFill[0];
            }

            healthBar.fillAmount = percentageHealth;

            previousHealth = percentageHealth;
        }
    }

    private void UpdatePowers()
    {
        if (powers.knownUpgrades.Count > 0)
        {
            // display slots equal to powers known
            int lastPowerIndex = powers.knownUpgrades.Count - 1;

            if (!powersDisplay[lastPowerIndex].activeInHierarchy)
            {
                powersDisplay[lastPowerIndex].transform.parent.gameObject.SetActive(true);
            }

            // for now, just change sprite to indicate if there is an upgrade slotted
            if (powers.slottedUpgrades.Count > 0 && powers.dirty)
            {
                for (int i = 0; i <= lastPowerIndex; i++)
                {
                    if (i < powers.slottedUpgrades.Count && powers.slottedUpgrades[i] != null)
                    {
                        powersDisplay[i].GetComponent<Image>().sprite = powerSprites[1];
                    }
                    else
                        powersDisplay[i].GetComponent<Image>().sprite = powerSprites[0];
                }
                powers.dirty = false;
            }
        }
    }

    private void UpdateFire()
    {
        if (previousFireTime != powers.fireTime)
        {
            float firePercentage = (float)powers.fireTime / powers.fireMax;

            if (firePercentage >= 1f && previousFireTime > powers.fireMax)
            {
                // do nothing
            }
            else if (firePercentage >= 1f && previousFireTime < powers.fireMax) // increasing past max
            {
                fireFill.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
                fireFill.GetComponent<Image>().color = new Color(0.95f, 0f, 0f, 0.7f);
            }
            else if (firePercentage < 1f && previousFireTime >= powers.fireMax) // decreasing from max
            {
                fireFill.GetComponent<Image>().color = new Color(0.75f, 0f, 0f, 0.7f);
            }
            else
                fireFill.GetComponent<RectTransform>().anchorMax = new Vector2(firePercentage,1f);

            previousFireTime = powers.fireTime;
        }
    }
}

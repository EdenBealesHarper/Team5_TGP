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

    private Dictionary<string, Sprite> powerSprites = new Dictionary<string, Sprite>();

    [SerializeField]
    private Button[] pauseButtons = new Button[3];

    private bool effectsDirty;

    [SerializeField]
    private GameObject effectsDisplay;
    private Dictionary<string, GameObject> effectImages = new Dictionary<string, GameObject>();
    private List<GameObject> currentEffects = new List<GameObject>();

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

            powerSprites.Add("blank",Resources.Load<Sprite>("UI/blank"));
            powerSprites.Add("occupied", Resources.Load<Sprite>("UI/occupied")); //fallback in case icon doesn't exist yet
            powerSprites.Add("ENGINE", Resources.Load<Sprite>("UI/iconENGINE"));
            powerSprites.Add("CHARGEJUMP", Resources.Load<Sprite>("UI/iconCHARGEJUMP"));
            powerSprites.Add("DOUBLEJUMP", Resources.Load<Sprite>("UI/iconDOUBLEJUMP"));

            effectImages.Add("burning", Resources.Load<GameObject>("UI/Effect_Fire"));

            previousHealth = 0f;
            previousFireTime = powers.fireMax;
            effectsDirty = true;

            InitialisePauseScreen();
        }
        else Debug.Log("Player character not found");
    }

    // Update is called once per frame
    void Update()
    {
        CheckPaused();
        UpdateHealth();
        UpdatePowers();
        UpdateEffects();
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

            
            if (powers.slottedUpgrades.Count > 0 && powers.dirty)
            {
                for (int i = 0; i <= lastPowerIndex; i++)
                {
                    if (i < powers.slottedUpgrades.Count && powers.slottedUpgrades[i] != null)
                    {
                        if (powerSprites.ContainsKey(powers.slottedUpgrades[i]))
                        {
                            powersDisplay[i].GetComponent<Image>().sprite = powerSprites[powers.slottedUpgrades[i]];
                        }
                        else
                            powersDisplay[i].GetComponent<Image>().sprite = powerSprites["occupied"];
                    }
                    else
                        powersDisplay[i].GetComponent<Image>().sprite = powerSprites["blank"];
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

    // TODO proper effects system; give effects individual sprites
    private void UpdateEffects()
    {
        // on fire
        if (powers.onFire && !currentEffects.Contains(effectImages["burning"]))
        {
            currentEffects.Add(effectImages["burning"]);
            effectsDirty = true;
        }
        else if (!powers.onFire && currentEffects.Contains(effectImages["burning"]))
        {
            currentEffects.Remove(effectImages["burning"]);
            effectsDirty = true;
        }

        if (effectsDirty)
        {
            // clear
            foreach (Transform child in effectsDisplay.transform)
            {
                Destroy(child.gameObject);
            }

            // refill
            if (currentEffects.Count > 0)
            {
                for (int i = 0; i < currentEffects.Count; i++)
                {
                    Instantiate(currentEffects[i], effectsDisplay.transform);
                }
            }

            effectsDirty = false;
        }
    }

    #region pause functions
    private void CheckPaused()
    {
        if (GameManager.Instance().isPaused() != pauseButtons[0].gameObject.activeInHierarchy)
        {
            pauseButtons[0].transform.parent.gameObject.SetActive(GameManager.Instance().isPaused());
        }
    }

    private void InitialisePauseScreen()
    {
        // resume
        pauseButtons[0].onClick.AddListener(() => GameManager.Instance().SetPause(false));

        // main menu
        pauseButtons[1].onClick.AddListener(GameManager.Instance().GameMenu);

        //quit
        pauseButtons[2].onClick.AddListener(GameManager.Instance().GameQuit);
    }
    #endregion
}

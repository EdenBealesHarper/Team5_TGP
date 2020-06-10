using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected float TotalHealth = 200;
    [SerializeField]
    protected float CurrentHealth = 200;

    [SerializeField]
    protected AudioClip injuredSFX;
    [SerializeField]
    protected AudioClip deathSFX;

    private void Start()
    {
        CurrentHealth = TotalHealth;
    }

  public virtual void TakeDamage(float Damage)
    {
        CurrentHealth -= Damage;
        Debug.Log("Damaged");
        if (CurrentHealth <= 0.0f)
        {
            //ToDo On Death event
            OnDeath();
        }
        else if (injuredSFX != null)
        {
            if (gameObject.tag == "Player")
                AudioManager.Instance().PlaySFXPlayer(injuredSFX);
            else
                AudioManager.Instance().PlaySFXEnemy(injuredSFX);
        }
    }

    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }
    public float GetTotalHealth()
    {
        return TotalHealth;
    }

    void OnDeath()
    {
        if (deathSFX != null)
        {
            if (gameObject.tag == "Player")
                AudioManager.Instance().PlaySFXPlayer(deathSFX);
            else
                AudioManager.Instance().PlaySFXEnemy(deathSFX);
        }
        Destroy(gameObject);

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected float CurrentHealth;
    [SerializeField]
    protected float TotalHealth;

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
        if (CurrentHealth <= 0)
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
        if (gameObject.tag == "Player")
        {
            AudioManager.Instance().PlaySFXPlayer(deathSFX);
        }
        else
        {
            AudioManager.Instance().PlaySFXEnemy(deathSFX);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected float CurrentHealth;
    [SerializeField]
    protected float TotalHealth;


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

    }
}

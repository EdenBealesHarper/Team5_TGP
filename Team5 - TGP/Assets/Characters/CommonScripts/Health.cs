using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected float CurrentHealth;
    protected float TotalHealth;
    

    void Start()
    {
        TotalHealth = 10f; // TODO placeholder
        CurrentHealth = TotalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            //ToDo On Death event
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
}

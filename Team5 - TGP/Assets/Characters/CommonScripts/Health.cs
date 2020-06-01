using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected float CurrentHealth;
    [SerializeField]
    protected float TotalHealth;

<<<<<<< HEAD
    void Start()
    {
        TotalHealth = 10f; // TODO placeholder
        CurrentHealth = TotalHealth;
    }
=======
>>>>>>> PlayerCharacter

    private void Start()
    {
        CurrentHealth = TotalHealth;
    }

<<<<<<< HEAD
    public void TakeDamage(float Damage)
=======
  public virtual void TakeDamage(float Damage)
>>>>>>> PlayerCharacter
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float Speed;
    public float Damage;

    public float Lifetime;

    private float Dir;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * Speed * Dir;
    }

    // Update is called once per frame
    void Update()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0) Destroy(gameObject);
    }

   public void SetDirection(int Mod)
    {
        Dir = Mod;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "CanBeDamaged")
        {
            DamageTarget(collision.gameObject.GetComponent<Health>());
        }
        Destroy(gameObject);
    }

    void DamageTarget(Health Target)
    {
        Target.TakeDamage(Damage);
    }
}

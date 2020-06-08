using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Walker : MonoBehaviour
{
    [SerializeField]
    GameObject RigidBodyObject;

    [SerializeField]
    GameObject Target;

    Animator Anim;
    Rigidbody2D rb;

    Health TargetHealth;




    bool bAttackTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
         rb = RigidBodyObject.GetComponent<Rigidbody2D>();

        TargetHealth=  Target.GetComponent<Health>();



    }

    // Update is called once per frame
    void Update()
    {
         Anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));


      

        if (bAttackTriggered)
        {
            Anim.ResetTrigger("Attack");
            bAttackTriggered = false;
        }
        


    }



   public void DoAttack()
    {
        Anim.SetTrigger("Attack");
        bAttackTriggered = true;
        TargetHealth.TakeDamage(10);

    }
}

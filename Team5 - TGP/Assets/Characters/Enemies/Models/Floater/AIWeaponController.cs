using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    [SerializeField]
    GameObject ProjectileObject;

    [SerializeField]
    GameObject ProjectilePosition;

    [SerializeField]
    GameObject Cannon;

    [SerializeField]
    GameObject Target;

    [SerializeField]
    float ActiveRange;

    [SerializeField]
    float FireRate;

    float FireTimer;

    bool bCanFire = true;

    float Direction = 1;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();
        if ( bCanFire && FireTimer <= 0)
        {
            FireWeapon();
            FireTimer = FireRate;
        }
        else
        {
            FireTimer -= Time.deltaTime;
        }
    }


    void FindTarget()
    {
        if (Vector2.Distance(transform.position, Target.transform.position) < ActiveRange)
        {
            Cannon.transform.LookAt(Target.transform.position);
            bCanFire = true;

        }
        else
        {
            bCanFire = false;
        }
    }


    void FireWeapon()
    {
        GameObject Proj = Instantiate(ProjectileObject, ProjectilePosition.transform.position, ProjectilePosition.transform.rotation);
        Proj.GetComponent<ProjectileBase>().SetDirection((int)Direction);
    }
}

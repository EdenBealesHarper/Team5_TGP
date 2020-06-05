﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_AI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public bool enableDetectionRange = true;
    public float detectionRange = 10f;

    public Transform enemyGFX;

    //private Animator Anim;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (enableDetectionRange == true)
        {
            if (IsTargetInDetRange())
                rb.AddForce(force);
        }
        else
        {
            rb.AddForce(force);
        }

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);

        }
        else if (rb.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    bool IsTargetInDetRange()
    {
        if (Vector2.Distance(target.position, rb.position) < detectionRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //public void Move(float Movement)
    //{
    //     // The Speed animator parameter is set to the absolute value of the horizontal input.
    //     // if (!bIsWeaponActive) Anim.SetFloat("Speed", Mathf.Abs(Movement));
    //     // else Anim.SetFloat("Speed", Movement);

    //    Anim.SetFloat("Speed", Mathf.Abs(Movement));
    //    Anim.SetInteger("Run", (int)(Movement * 10));

    //    // Move the character
    //    rb.velocity = new Vector2((Movement * MaxSpeed) * SpeedModifier, rb.velocity.y);

    //    // If the input is moving the player right and the player is facing left...
    //    if (Movement > 0 && !FacingRight)
    //    {
    //        // ... flip the player.
    //        Flip();
    //    }
    //    // Otherwise if the input is moving the player left and the player is facing right...
    //    else if (Movement < 0 && FacingRight)
    //    {
    //        // ... flip the player.
    //        Flip();
    //    }
    //}
}

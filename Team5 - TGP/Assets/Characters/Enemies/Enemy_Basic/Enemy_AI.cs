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
    public float attackRange = 3f;
    public float attackCooldown = 1f;
    public int attackDamage = 20;

    public Transform enemyGFX;
    public AI_Walker weaponController;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    float attackTime = 0f;

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
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
            reachedEndOfPath = false;

        if (attackTime > 0)
            attackTime -= Time.deltaTime;

        if (attackTime < 0)
            attackTime = 0;

        CheckRange();

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (enableDetectionRange == true)
        {
            if (IsTargetInDetRange())
                rb.AddForce(force);
        }
        else
            rb.AddForce(force);

        if (distance < nextWaypointDistance)
            currentWaypoint++;

        if (rb.velocity.x >= 0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (rb.velocity.x <= -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    bool IsTargetInDetRange()
    {
        if (Vector2.Distance(target.position, rb.position) < detectionRange)
            return true;
        else
            return false;
    }

    void Attack()
    {
        int i = 0;
        if (attackTime == 0)
        {
            Debug.Log("Attack " + (i+1));
            weaponController.DoAttack(attackDamage);
            attackTime = attackCooldown;
        }
    }

    void CheckRange()
    {
        //if ((Vector2.Distance(target.position, rb.position) < attackRange))
        if ((target.transform.position - rb.transform.position).sqrMagnitude < attackRange * attackRange)
            Attack();
    }
}

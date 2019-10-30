﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float speed = .5f;

    private int health = 5;

    private Transform currentWaypoint;
    private int currentWaypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = MapGenerator.Instance.Waypoints[currentWaypointIndex];
        transform.LookAt(currentWaypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypoint is null)
            return;

        if (Vector3.Distance(transform.position, currentWaypoint.position) < .1f)
        {
            NextWaypoint();
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }

    void NextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex >= MapGenerator.Instance.Waypoints.Count)
        {
            currentWaypoint = null;
            return;
        }

        currentWaypoint = MapGenerator.Instance.Waypoints[currentWaypointIndex];
        transform.LookAt(currentWaypoint.position);
    }

    public void Damage(int damage)
    {
        health -= damage;
    }
}

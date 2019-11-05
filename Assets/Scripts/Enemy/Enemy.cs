using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float speed = .5f;
    [SerializeField]
    private float startHealth = 100;
    private float health;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Canvas UI;

    private Transform currentWaypoint;
    private int currentWaypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = MapGenerator.Instance.Waypoints[currentWaypointIndex];
        transform.LookAt(currentWaypoint.position);
        health = startHealth;
        UI.gameObject.SetActive(false);
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
        
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

        // Camera billboard effect
        UI.transform.LookAt(UI.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
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

    public void Damage(float damage)
    {
        health -= damage;

        healthBar.fillAmount = health / startHealth;

        if (health < startHealth)
            UI.gameObject.SetActive(true);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
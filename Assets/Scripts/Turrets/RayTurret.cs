using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayTurret : Turret
{

    [Header("Laser settings")]
    [SerializeField]
    private float laserWidth = .01f;
    [SerializeField]
    private Color laserColor = Color.green;
    [SerializeField]
    private Light light;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;

        lineRenderer.positionCount = 2;

        light.color = laserColor;

        InvokeRepeating("UpdateTarget", 0f, 0.3f);
    }

    private void Update()
    {
        if (target != null)
        {
            LookAtTarget();
            Laser();
        }
        else
        {
            DeactivateLaser();
        }
    }

    void Laser()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            light.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        enemyComponent.Damage(hitDamage * Time.deltaTime);
    }

    void DeactivateLaser()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
            light.enabled = false;
        }
    }
}

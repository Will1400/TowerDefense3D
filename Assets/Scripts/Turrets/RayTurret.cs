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
    private float maxLength = 50.0f;
    [SerializeField]
    private float updateRate = .005f;
    [SerializeField]
    private Color laserColor = Color.green;
    private LineRenderer lineRenderer;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;

        lineRenderer.positionCount = 2;

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
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);


        target.gameObject.GetComponent<Enemy>().Damage(hitDamage * Time.deltaTime);
    }

    void DeactivateLaser()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
        }
    }
}

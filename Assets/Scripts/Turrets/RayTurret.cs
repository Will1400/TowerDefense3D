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


    private Transform target;

    private float nextUpdate;
    private LineRenderer lineRenderer;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        lineRenderer.positionCount = 2;
        InvokeRepeating("UpdateTarget", 0f, 0.3f);

    }



    void UpdateTarget()
    {
        List<Collider> nearbyObjects = Physics.OverlapSphere(firePoint.position, range / 2).ToList();
        nearbyObjects.RemoveAll(x => x.GetComponent<Enemy>() is null);

        float shortestDistance = Mathf.Infinity;
        foreach (var enemy in nearbyObjects)
        {
            float distanceToEnemy = Vector3.Distance(firePoint.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                target = enemy.transform;
            }
        }
    }


    private void Update()
    {
        if (target != null)
        {
            LookAtTarget();
            ActivateLaser();
        }
        else
        {
            DeactivateLaser();
        }
    }

    void LookAtTarget()
    {
        //Vector3 dir = target.position - transform.position;
        //Quaternion lookRotation = Quaternion.LookRotation(dir);
        //Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnRate).eulerAngles;
        //partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        partToRotate.LookAt(target.position);
        partToRotate.eulerAngles = new Vector3(-90, partToRotate.eulerAngles.y, 90);
    }

    void ActivateLaser()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

    void DeactivateLaser()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.enabled = false;
        }
    }
}

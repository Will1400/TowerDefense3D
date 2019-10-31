using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Turret : MonoBehaviour
{

    public string Name;
    public Sprite Icon;

    public int Cost { get; set; }

    [SerializeField]
    protected float hitDamage = 5;
    [SerializeField]
    protected float turnRate = 20;
    [SerializeField]
    protected float range = 10;

    [SerializeField]
    protected Transform firePoint;
    [SerializeField]
    protected Transform partToRotate;
    [SerializeField]
    public Vector3 offset = new Vector3(0, .55f, 0);

    protected Transform target;

    protected void LookAtTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnRate).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(transform.eulerAngles.x, rotation.y, transform.eulerAngles.z);
    }

    protected void UpdateTarget()
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
}
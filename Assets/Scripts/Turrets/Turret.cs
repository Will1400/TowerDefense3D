using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public abstract class Turret : MonoBehaviour
{

    public string Name;
    public Sprite Icon;

    public int Cost = 100;

    public int UpgradeTier = 0;
    public bool CanBeUpgraded => UpgradeTier < tiers.Count;

    [SerializeField]
    protected List<UpgradeInTier> tiers;

    [Header("Stats")]
    [Space(10)]
    [SerializeField]
    protected float hitDamage = 5;
    [SerializeField]
    protected float turnRate = 20;
    [SerializeField]
    protected float range = 3;

    [Space(10)]
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
        List<Collider> nearbyObjects = Physics.OverlapSphere(firePoint.position, range).ToList();
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

    public void Upgrade()
    {
        if (UpgradeTier < tiers.Count)
        {
            UpgradeTier++;
            ApplyUpgrades();
        }
    }

    protected virtual List<Upgrade> ApplyUpgrades()
    {
        List<Upgrade> upgradesToApply = tiers.Where(x => x.Tier == UpgradeTier).SelectMany(x => x.Upgrades).ToList();
        List<Upgrade> remainingUpgrades = new List<Upgrade>(upgradesToApply);

        foreach (Upgrade upgrade in upgradesToApply)
        {
            bool applied = true;
            switch (upgrade.AppliesTo)
            {
                case "hitDamage":
                    hitDamage += upgrade.Amount;
                    break;
                case "range":
                    range += upgrade.Amount;
                    break;
                case "turnRate":
                    turnRate += upgrade.Amount;
                    break;
                default:
                    applied = false;
                    break;
            }
            if (applied)
                remainingUpgrades.Remove(upgrade);
        }
        return remainingUpgrades;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTurret : Turret
{
    [SerializeField]
    protected float fireRate = .4f;
    [SerializeField]
    protected GameObject bulletPrefab;


    private float nextFire;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.3f);
    }

    private void Update()
    {
        if (target != null)
        {
            LookAtTarget();
            if (nextFire < Time.time)
            {
                Shoot();
            }
        }
    }

    protected void Shoot()
    {

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.InitializeBullet(target, hitDamage);
        nextFire = Time.time + fireRate;
    }

    protected override List<Upgrade> ApplyUpgrades()
    {
        List<Upgrade> upgradesToApply = base.ApplyUpgrades();
        List<Upgrade> remainingUpgrades = upgradesToApply;

        foreach (Upgrade upgrade in upgradesToApply)
        {
            bool applied = true;
            switch (upgrade.AppliesTo)
            {
                case "fireRate":
                    hitDamage -= Mathf.Abs(upgrade.Amount);
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
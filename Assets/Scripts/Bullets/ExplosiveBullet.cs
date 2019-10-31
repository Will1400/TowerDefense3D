using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ExplosiveBullet : Bullet
{
    [SerializeField]
    protected float radius = 2;

    protected override void Hit(GameObject enemy)
    {
        List<Collider> hits = Physics.OverlapSphere(enemy.transform.position, radius).Where(x => x.GetComponent<Enemy>() != null).ToList();

        foreach (var item in hits)
        {
            item.GetComponent<Enemy>().Damage(hitDamage);
        }

        Destroy(gameObject);
    }
}
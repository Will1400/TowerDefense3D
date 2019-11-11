using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AuraTurret : Turret
{
    [Header("Aura Specific")]
    [Space(10)]
    [SerializeField]
    private Effect effectToApply;
    [SerializeField]
    private float rate = .1f;

    private void Start()
    {
        StartCoroutine("ApplyAura");
    }

    IEnumerator ApplyAura()
    {
        while (true)
        {
            List<GameObject> enemies = GetNearbyEnemies();

            foreach (Enemy item in enemies.Select(x => x.GetComponent<Enemy>()))
            {
                item.ApplyEffect(effectToApply.Clone());
            }

            yield return new WaitForSeconds(rate);
        }
    }


    private List<GameObject> GetNearbyEnemies()
    {
        return Physics.OverlapSphere(transform.position, range).Where(x => x.GetComponent<Enemy>() != null).Select(x => x.gameObject).ToList();
    }
}
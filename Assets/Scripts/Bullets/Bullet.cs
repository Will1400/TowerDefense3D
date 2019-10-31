using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    protected Transform target;
    protected float hitDamage;

    public virtual void InitializeBullet(Transform _target, float _hitDamage, float _speed = 0)
    {
        target = _target;
        hitDamage = _hitDamage;
        if (_speed != 0)
        speed = _speed;

        transform.LookAt(target);
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void Hit(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().Damage(hitDamage);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Hit(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
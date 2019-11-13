using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Reflection;
using System.Linq;
using Unity.Mathematics;

public class Enemy : MonoBehaviour, IDamageable
{
    public float speed = .5f;

    [SerializeField]
    private float startHealth = 100;
    public float health;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Canvas UI;

    private Transform currentWaypoint;
    private int currentWaypointIndex;

    private List<Effect> effects = new List<Effect>();

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = MapGenerator.Instance.Waypoints[currentWaypointIndex];
        transform.LookAt(currentWaypoint.position);
        if (health == 0)
            health = startHealth;
        else
            startHealth = health;
        UI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypoint is null || transform is null)
            return;

        if (Vector3.Distance(transform.position, currentWaypoint.position) < .1f)
        {
            NextWaypoint();
        }

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

        // Camera billboard effect
        UI.transform.LookAt(UI.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);


        // Effects
        effects.ForEach(x => x.Duration -= Time.deltaTime);
        effects.Where(x => x.Duration <= 0).ToList().ForEach(x => RemoveEffect(x));

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

    public void ApplyEffect(Effect effect)
    {
        if (effects.Contains(effect))
        {
            effects.Find(x => x.Equals(effect)).Duration = effect.Duration;
        }
        else
        {
            effects.Add(effect);
            Apply(effect);
        }

        void Apply(Effect effectToApply)
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var item in fields)
            {
                if (item.Name == effectToApply.AppliesTo)
                {
                    float currentValue = (float)item.GetValue(this);
                    if (effectToApply.EffectType == EffectType.Buff)
                        item.SetValue(this, currentValue + (currentValue * (effectToApply.Amount / 100)));
                    else
                        item.SetValue(this, currentValue - (currentValue * (effectToApply.Amount / 100)));

                }
            }
        }
    }

    void RemoveEffect(Effect effect)
    {
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var item in fields)
        {
            if (item.Name == effect.AppliesTo)
            {
                float currentValue = (float)item.GetValue(this);
                if (effect.EffectType == EffectType.Buff)
                    item.SetValue(this, (float)item.GetValue(this) - (effect.Amount / 100));
                else
                    item.SetValue(this, (float)item.GetValue(this) + (effect.Amount / 100));

                effects.Remove(effect);
            }
        }
    }
}
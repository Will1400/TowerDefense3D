using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{

    public string Name;

    public int Cost { get; set; }

    [SerializeField]
    protected int hitDamage = 5;

    [SerializeField]
    protected float turnRate = 20;
    [SerializeField]
    protected float range = 10;

    [SerializeField]
    protected Transform firePoint;
    [SerializeField]
    protected Transform partToRotate;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

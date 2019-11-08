using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private int startHealth = 20;
    public int Health = 20;

    public UnityEvent HealthChanged;
    public UnityEvent GameLost;
    public UnityEvent GameWon;

    void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);

        HealthChanged = new UnityEvent();
        GameLost = new UnityEvent();
        GameWon = new UnityEvent();
    }

    private void Start()
    {
        MapGenerator.Instance.MapRendered.AddListener(OnMapRendered);
    }

    void OnMapRendered()
    {
        if (Health <= 0)
            Health = startHealth;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        HealthChanged.Invoke();
        if (Health <= 0)
        {
            GameLost.Invoke();
        }
    }
}
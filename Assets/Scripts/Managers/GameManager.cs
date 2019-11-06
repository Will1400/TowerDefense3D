using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Health = 20;

    public UnityEvent HealthChanged;

    void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);

        HealthChanged = new UnityEvent();
    }

    private void Start()
    {
        MapGenerator.Instance.MapRendered.AddListener(OnMapRendered);
    }

    void OnMapRendered()
    {
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        HealthChanged.Invoke();
    }
}
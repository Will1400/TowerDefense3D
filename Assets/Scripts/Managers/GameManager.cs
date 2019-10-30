using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    

    void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);

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

    }
}

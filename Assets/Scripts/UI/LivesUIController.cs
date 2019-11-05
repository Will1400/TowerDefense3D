using UnityEngine;
using System.Collections;
using TMPro;

public class LivesUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private GameObject canvas;


    void Start()
    {
        GameManager.Instance.HealthChanged.AddListener(UpdateHealth);
        UpdateHealth();
    }

    void Update()
    {
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    void UpdateHealth()
    {
        healthText.text = $"Lives: {GameManager.Instance.Health}";
    }
}
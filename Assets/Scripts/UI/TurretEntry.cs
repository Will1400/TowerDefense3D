using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretEntry : MonoBehaviour
{
    private int turretIndex;
    [SerializeField]
    private TextMeshProUGUI turretNameText;
    [SerializeField]
    private Image turretDisplayIcon;
    [SerializeField]
    private Button selectTurretButton;

    private void Start()
    {
        selectTurretButton.onClick.AddListener(() =>
        {
            BuildManager.Instance.SelectTurret(turretIndex);
        });
    }

    public void Initialize(int _turretIndex, string _turretName, Sprite _icon)
    {
        turretIndex = _turretIndex;
        turretNameText.text = _turretName;
        turretDisplayIcon.sprite = _icon;
    }
}
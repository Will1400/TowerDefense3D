using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeUIController : MonoBehaviour
{
    public static NodeUIController Instance;

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private Button sellButton;

    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private TextMeshProUGUI upgradeCostText;

    private Tile selectedTile;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);

        canvas.SetActive(false);
    }


    private void Update()
    {
        if (selectedTile is null)
            return;

        // Billboard effect
        canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void OnClickSell()
    {
        if (selectedTile is null)
            return;

        selectedTile.SellTurret();
        DeselectTile();
    }

    public void OnClickUpgrade()
    {
        if (selectedTile is null)
            return;

        selectedTile.UpgradeTurret();
        DeselectTile();
    }


    public void DeselectTile()
    {
        canvas.SetActive(false);
        selectedTile = null;
    }

    public void SelectTile(Tile tile)
    {
        selectedTile = tile;
        Vector3 offset = tile.GetOffset();
        canvas.transform.position = tile.transform.position + Vector3.up;
        canvas.SetActive(true);
        Turret tileTurret = tile.Turret.GetComponent<Turret>();
        upgradeCostText.text = (tileTurret.Cost / (3 - tileTurret.UpgradeTier)).ToString();
    }
}
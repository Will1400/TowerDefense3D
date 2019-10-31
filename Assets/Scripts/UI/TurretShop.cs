using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurretShop : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollView;
    [SerializeField]
    private GameObject scrollContent;
    [SerializeField]
    private GameObject turretEntryPrefab;


    private void Start()
    {
        for (int i = 0; i < BuildManager.Instance.TurretPrefabs.Count; i++)
        {
            var item = BuildManager.Instance.TurretPrefabs[i];
            GameObject turretObj = Instantiate(turretEntryPrefab);
            turretObj.transform.SetParent(scrollContent.transform, false);
            turretObj.transform.localPosition += new Vector3(320, 0) * i;
            var turretComponent = item.GetComponent<Turret>();
            turretObj.GetComponent<TurretEntry>().Initialize(i, turretComponent.Name, turretComponent.Icon);

            if (i > 3)
            {
                scrollContent.GetComponent<RectTransform>().sizeDelta += new Vector2( 300,0);
            }
        }

        //scrollView.horizontalNormalizedPosition = 1;
    }
}
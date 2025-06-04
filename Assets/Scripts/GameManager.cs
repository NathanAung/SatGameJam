using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int[] turretPrices;
    public int[] upgradePrices;
    public int money = 10;
    public List<TurretBase> turretBases;
    public int turretsPlaced = 0;

    private bool building = true;

    [SerializeField] LayerMask clickableLayer;


    // Start is called before the first frame update
    void Start() {
        UpdatePrices();
    }

    // Update is called once per frame
    void Update() {
        if (building) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer)) {
                    Debug.Log("Clicked on: " + hit.transform.parent.name);
                    hit.transform.parent.GetComponent<TurretBase>().OnBaseClicked();
                }
            }
        }
    }

    public void UpdatePrices() {
        for (int i = 0; i < turretBases.Count; i++) {
            if (!turretBases[i].turretPlaced) {
                // set buy price
                turretBases[i].UpdatePrice(turretPrices[turretsPlaced]);
            }
            else {
                // set upgrade price
                turretBases[i].UpdatePrice(upgradePrices[turretBases[i].turretLevel]);
            }
        }
    }
}

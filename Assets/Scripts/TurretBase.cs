using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretBase : MonoBehaviour {
    GameManager gameManager;
    [SerializeField] GameObject turretPrefab;
    private Turret turret;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] GameObject clickArea;

    public int price = 1;
    public bool turretPlaced = false;
    public int turretLevel = 0;
    [SerializeField] int[] turretDamages;
    [SerializeField] float[] turretCDs;


    // Start is called before the first frame update
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnBaseClicked() {
        if (gameManager.money < price || turretLevel >= 3) return;

        if (!turretPlaced) {
            // place turret
            GameObject placedTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
            turret = placedTurret.GetComponent<Turret>();
            gameManager.turretsPlaced += 1;
            turretPlaced = true;
        }
        else {
            // upgrade turret
            turretLevel += 1;
            turret.damage = turretDamages[turretLevel];
            turret.attackTime = turretCDs[turretLevel];
            turret.UpdateBody(turretLevel);
            Debug.Log("Upgraded");
        }

        gameManager.money -= price;
        gameManager.UpdatePrices();

        Debug.Log("Turret clicked");
    }

    public void UpdatePrice(int p) {
        if (turretLevel >= 3) return;

        price = p;
        priceText.text = "$" + price;
    }
}

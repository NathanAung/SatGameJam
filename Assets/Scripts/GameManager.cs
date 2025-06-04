using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public int[] turretPrices;
    public int[] upgradePrices;
    public int money = 1;
    public int score = 0;
    public List<TurretBase> turretBases;
    public int turretsPlaced = 0;
    private bool building = true;

    [SerializeField] LayerMask clickableLayer;
    public bool gameStarted = false;
    public bool gameClear = false;
    public bool gameOver = false;


    // Start is called before the first frame update
    void Start() {
        UpdatePrices();
    }


    // Update is called once per frame
    void Update() {
        if (gameStarted) {
            if (!gameClear && !gameOver) {
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
            else {
                // reload scene if won or lost
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                gameStarted = true;
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


    public void OnEnemyKill(int s, int m) {
        score += s;
        // update score text UI
        money += m;
        // update money text UI
    }


    public void GameClear() {
        if (gameClear || gameOver) return;

        Debug.Log("Game Clear");
    }


    public void GameOver() {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("Game Over");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] int score = 10;

    void OnTriggerEnter2D(Collider2D collision) {
        GameManager.UpdateScore(score);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    [SerializeField] GameManager gameManager;
    private bool clear = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (clear) return;

        Debug.Log("Game Clear");
        collision.GetComponent<PlayerMovement>().playerActive = false;
        collision.GetComponent<PlayerMovement>().DisableCollision();
        gameManager.GameClear();
        clear = true;
    }
}

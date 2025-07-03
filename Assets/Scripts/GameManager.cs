using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] PlayerCards playerCards;
    [SerializeField] PlayerCards enemyCards;
    public int currentTurn = 0;
    [SerializeField] GameObject startText;
    public GameObject winText;
    [SerializeField] GameObject loseText;
    [SerializeField] GameObject drawText;
    private bool gameStarted = false;
    public bool gameOver = false;


    // Start is called before the first frame update
    void Start() {

    }


    // Update is called once per frame
    void Update() {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space)) {
            startText.SetActive(false);
            StartCoroutine(DrawFirstCards());
            playerCards.DrawCards();
            enemyCards.DrawCards();
            gameStarted = true;
        }
        else if (gameOver && Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public IEnumerator NextTurn() {
        yield return new WaitForSeconds(0.5f);

        // player turn to enemy turn
        if (currentTurn == 1) {
            if (playerCards.handPoints < enemyCards.handPoints) {
                EndGame(1);
            }
            else {
                if (playerCards.handPoints == enemyCards.handPoints) {
                    playerCards.EraseHand();
                    enemyCards.EraseHand();
                    if (playerCards.cardList.Count == 0 && enemyCards.cardList.Count == 0) {
                        EndGame(2);
                        yield break;
                    }
                }
                currentTurn = 2;
                playerCards.canSelectCards = false;
                if (enemyCards.cardList.Count > 0) {
                    enemyCards.SearchBeatableCard();
                }
                else {
                    EndGame(0);
                }
            }
        }
        // enemy turn to player turn
        else {
            if (enemyCards.handPoints < playerCards.handPoints) {
                EndGame(0);
            }
            else {
                if (playerCards.handPoints == enemyCards.handPoints) {
                    playerCards.EraseHand();
                    enemyCards.EraseHand();
                    if (playerCards.cardList.Count == 0 && enemyCards.cardList.Count == 0) {
                        EndGame(2);
                        yield break;
                    }
                }
                currentTurn = 1;
                if (playerCards.cardList.Count > 0) {
                    playerCards.canSelectCards = true;
                }
                else {
                    EndGame(1);
                }
            }
        }
    }


    public IEnumerator DrawFirstCards() {
        do {
            yield return new WaitForSeconds(0.5f);
            playerCards.EraseHand();
            enemyCards.EraseHand();
            playerCards.DrawFirstCard();
            enemyCards.DrawFirstCard();
        } while (playerCards.handPoints == enemyCards.handPoints);

        if (playerCards.handPoints < enemyCards.handPoints) {
            currentTurn = 2;
        }
        else {
            currentTurn = 1;
        }
        StartCoroutine(NextTurn());
    }


    public void EndGame(int type) {
        switch (type) {
            case 0: // win
                Debug.Log("You Win");
                enemyCards.ShowCards();
                winText.SetActive(true);
                break;
            case 1: // lose
                Debug.Log("You Lose");
                enemyCards.ShowCards();
                loseText.SetActive(true);
                break;
            case 2: // draw
                Debug.Log("Draw");
                drawText.SetActive(true);
                break;
        }
        gameOver = true;
    }
}

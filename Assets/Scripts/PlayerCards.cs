using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCards : MonoBehaviour {
    [SerializeField] GameManager gameManager;
    [SerializeField] bool player = true;
    [SerializeField] GameObject card;
    [SerializeField] List<GameObject> cardList;
    [SerializeField] List<GameObject> handList;
    [SerializeField] GameObject playerCardPos;
    [SerializeField] GameObject playerHandPos;
    [SerializeField] Vector2[] cardPositions = new Vector2[10];
    [SerializeField] Vector2[] handPositions = new Vector2[10];
    [SerializeField] Vector2 deckPos = new Vector2(-7f, -3.5f);
    [SerializeField] LayerMask clickableLayer;
    public int handPoints = 0;
    public bool canSelectCards = true;
    [SerializeField] PlayerCards opponentCards; // player cards, for enemy


    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < playerCardPos.transform.childCount; i++) {
            cardPositions[i] = playerCardPos.transform.GetChild(i).transform.position;
            handPositions[i] = playerHandPos.transform.GetChild(i).transform.position;
        }

        DrawCards();
    }

    // Update is called once per frame
    void Update() {
        if (player && canSelectCards) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, clickableLayer);

                if (hit) {
                    Card card = hit.transform.GetComponent<Card>();
                    Debug.Log("Clicked on: " + card.posInList);
                    if (!card.inHand) {
                        AddToHand(card.posInList);
                    }


                }
            }

        }
    }

    public void DrawCards() {
        for (int i = 0; i < 10; i++) {
            GameObject c = Instantiate(card, deckPos, Quaternion.identity, gameObject.transform);
            int cardNo = Random.Range(0, 12);
            cardList.Add(c);
            c.GetComponent<Card>().SetUpCard(cardNo, player);
            c.GetComponent<Card>().playerCards = this;
        }

        cardList = cardList.OrderByDescending(obj => obj.GetComponent<Card>().points)
                   .ThenBy(obj => obj.GetComponent<Card>().cardType).ToList();

        for (int i = 0; i < 10; i++) {
            cardList[i].GetComponent<Card>().MoveCard(cardPositions[i], i);
        }
    }


    public void AddToHand(int cardNo) {
        GameObject c = cardList[cardNo];
        cardList.RemoveAt(cardNo);
        handList.Insert(0, c);

        for (int i = 0; i < cardList.Count; i++) {
            cardList[i].GetComponent<Card>().MoveCard(cardPositions[i], i);
        }
        for (int i = 0; i < handList.Count; i++) {
            handList[i].GetComponent<Card>().MoveCard(handPositions[i], i);
        }


        c.GetComponent<Card>().inHand = true;
        c.GetComponent<Card>().canSelect = false;
        c.GetComponent<Card>().selected = true;
        handPoints += c.GetComponent<Card>().points;
    }


    public void RemoveFromHand() {
        GameObject c = handList[0];
        handPoints -= c.GetComponent<Card>().points;

        handList.RemoveAt(0);
        Destroy(c);
        for (int i = 0; i < handList.Count; i++) {
            handList[i].GetComponent<Card>().MoveCard(handPositions[i], i);
        }
    }

    public void RemoveEnemyCard() {
        opponentCards.RemoveFromHand();
        gameManager.NextTurn();
    }

    public void NextTurn() {
        gameManager.NextTurn();
    }


    public void SearchBeatableCard() {
        int oppPoint = opponentCards.handPoints;

        int cardToUse = -1;
        int cardPoints = 500;
        for (int i = 0; i < cardList.Count; i++) {
            Card c = cardList[i].GetComponent<Card>();
            if (c.points + handPoints > oppPoint && c.points < cardPoints) {
                cardToUse = i;
                cardPoints = c.points;
            }
        }

        if (cardToUse == -1) {
            SearchSpecialCard();
        }
        else {
            AddToHand(cardToUse);
        }
    }

    private void SearchSpecialCard() {
        int cardToUse = -1;
        for (int i = 0; i < cardList.Count; i++) {
            Card c = cardList[i].GetComponent<Card>();
            if (c.cardType == 1 || c.cardType == 2) {
                cardToUse = i;
                break;
            }
        }

        if (cardToUse == -1) {
            Debug.Log("You Win!");
        }
        else {
            AddToHand(cardToUse);
        }
    }
}

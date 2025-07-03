using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCards : MonoBehaviour {
    [SerializeField] GameManager gameManager;
    [SerializeField] bool player = true;
    [SerializeField] GameObject card;   // card prefab
    public List<GameObject> cardList; // list of playing cards
    [SerializeField] List<GameObject> handList; // list of cards in hand
    [SerializeField] GameObject playerCardPos;  // playing card positions parent
    [SerializeField] GameObject playerHandPos;  // hand positions parent
    [SerializeField] Vector2[] cardPositions = new Vector2[10];
    [SerializeField] Vector2[] handPositions = new Vector2[11];
    [SerializeField] Vector2 deckPos = new Vector2(-7f, -3.5f); // deck pos to spawn cards from
    [SerializeField] LayerMask clickableLayer;
    public TextMeshProUGUI totalText;
    public int handPoints = 0;
    public bool canSelectCards = false;
    [SerializeField] PlayerCards opponentCards; // player cards, for enemy


    void Awake() {
        for (int i = 0; i < playerCardPos.transform.childCount; i++) {
            cardPositions[i] = playerCardPos.transform.GetChild(i).transform.position;
        }
        for (int i = 0; i < playerHandPos.transform.childCount; i++) {
            handPositions[i] = playerHandPos.transform.GetChild(i).transform.position;
        }
    }


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (player && canSelectCards) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, clickableLayer);

                if (hit) {
                    Card card = hit.transform.GetComponent<Card>();
                    //Debug.Log("Clicked on: " + card.posInList);
                    if (!card.inHand && card.playerCard) {
                        AddToHand(card.posInList);
                    }
                }
            }

        }
    }


    // draw 10 cards initially
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

    public void DrawFirstCard() {
        GameObject c = Instantiate(card, deckPos, Quaternion.identity, gameObject.transform);
        int cardNo = Random.Range(0, 12);
        handList.Add(c);
        c.GetComponent<Card>().SetUpCard(cardNo, player);
        c.GetComponent<Card>().playerCards = this;
        c.GetComponent<Card>().inHand = true;
        c.GetComponent<Card>().effectRemoved = true;
        handPoints += c.GetComponent<Card>().points;

        c.GetComponent<Card>().MoveCard(handPositions[0], 0);

        totalText.text = handPoints.ToString();
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

        if (player)
            canSelectCards = false;
        totalText.text = handPoints.ToString();
    }

    public void EraseHand() {
        for (int i = 0; i < handList.Count; i++) {
            Destroy(handList[i].gameObject);
        }
        handList = new List<GameObject>();
        handPoints = 0;
        totalText.text = handPoints.ToString();
    }


    // remove the first card of own hand
    public void RemoveFromHand() {
        if (handList.Count <= 0) return;

        GameObject c = handList[0];
        handPoints -= c.GetComponent<Card>().points;

        handList.RemoveAt(0);
        Destroy(c);
        for (int i = 0; i < handList.Count; i++) {
            handList[i].GetComponent<Card>().MoveCard(handPositions[i], i);
        }

        totalText.text = handPoints.ToString();
    }


    public void RemoveEnemyCard() {
        opponentCards.RemoveFromHand();
        StartCoroutine(gameManager.NextTurn());
    }


    // mirror card
    public void SwapHand() {
        List<GameObject> tempHand = new List<GameObject>(handList);
        int tempPoints = handPoints;
        handList = new List<GameObject>(opponentCards.handList);
        opponentCards.handList = new List<GameObject>(tempHand);

        handPoints = opponentCards.handPoints;
        opponentCards.handPoints = tempPoints;

        for (int i = 0; i < handList.Count; i++) {
            handList[i].GetComponent<Card>().MoveCard(handPositions[i], i);
        }
        totalText.text = handPoints.ToString();

        for (int i = 0; i < opponentCards.handList.Count; i++) {
            opponentCards.handList[i].GetComponent<Card>().MoveCard(opponentCards.handPositions[i], i);
        }
        opponentCards.totalText.text = opponentCards.handPoints.ToString();

        StartCoroutine(gameManager.NextTurn());
    }


    public void NextTurn() {
        StartCoroutine(gameManager.NextTurn());
    }


    // enemy action
    public void SearchBeatableCard() {
        int oppPoint = opponentCards.handPoints;

        int cardToUse = -1;
        int cardPoints = 500;
        for (int i = 0; i < cardList.Count; i++) {
            Card c = cardList[i].GetComponent<Card>();
            if (c.cardType == 0) {
                if (c.points + handPoints > oppPoint && c.points < cardPoints) {
                    cardToUse = i;
                    cardPoints = c.points;
                }
                else if (c.points + handPoints >= oppPoint && cardToUse == -1) {
                    cardToUse = i;
                    cardPoints = c.points;
                }
            }
        }

        if (cardToUse == -1) {
            SearchSpecialCard();
        }
        else {
            AddToHand(cardToUse);
        }
    }


    // enemy action
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
            gameManager.EndGame(0);
        }
        else {
            AddToHand(cardToUse);
        }
    }

    public void ShowCards() {
        for (int i = 0; i < cardList.Count; i++) {
            cardList[i].GetComponent<Card>().ShowHideCard(true);
        }
    }
}

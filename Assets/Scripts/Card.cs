using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] Sprite[] cardSprites;
    [SerializeField] SpriteRenderer cardIcon;
    [SerializeField] TextMeshPro cardText;
    GameObject cardBack;
    public bool playerCard = true;
    public int cardType = 0;    // 0 -normal, 1 - bolt, 2 - mirror
    public int points = 1;
    public bool faceDown = true;
    public bool inHand = false;
    public Vector2 targetPos;
    private float cardMoveSpeed = 20f;


    void Awake()
    {
        cardBack = transform.GetChild(0).gameObject;
        cardIcon = transform.GetChild(2).GetComponent<SpriteRenderer>();
        cardText = transform.GetChild(4).GetComponent<TextMeshPro>();
        ShowHideCard(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, targetPos) > 0.001f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, cardMoveSpeed * Time.deltaTime);
        }
        else if ((playerCard || (!playerCard && inHand)) && faceDown)
        {
            cardBack.SetActive(false);
        }

    }

    public void ShowHideCard(bool show)
    {
        cardBack.SetActive(show);
        faceDown = show;
    }

    public void SetUpCard(int cardNo, bool player)
    {
        if (cardNo > 9)
        {
            if (cardNo == 10)
            {
                cardType = 1;
                cardText.text = "B";
            }
            else
            {
                cardType = 2;
                cardText.text = "M";
            }
        }
        else
        {
            cardText.text = (cardNo + 1).ToString();
            points = cardNo + 1;
        }
        cardIcon.sprite = cardSprites[cardNo];
        playerCard = player;
    }
}

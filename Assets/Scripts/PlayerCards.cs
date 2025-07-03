using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCards : MonoBehaviour
{
    [SerializeField] bool player = true;
    [SerializeField] GameObject card;
    [SerializeField] GameObject playerCards;
    [SerializeField] List<GameObject> cardList;
    [SerializeField] GameObject playerCardPos;
    [SerializeField] Vector2[] cardPositions = new Vector2[10];
    [SerializeField] Vector2 deckPos = new Vector2(-7f, -3.5f);


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < playerCardPos.transform.childCount; i++)
        {
            cardPositions[i] = playerCardPos.transform.GetChild(i).transform.position;
        }

        DrawCards();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawCards()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject c = Instantiate(card, deckPos, Quaternion.identity, playerCards.transform);
            int cardNo = Random.Range(0, 12);
            cardList.Add(c);
            c.GetComponent<Card>().SetUpCard(cardNo, player);
        }

        cardList = cardList.OrderByDescending(obj => obj.GetComponent<Card>().points).ToList();
    }
}

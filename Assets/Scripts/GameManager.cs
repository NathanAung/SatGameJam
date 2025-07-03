using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerCards playerCards;
    [SerializeField] PlayerCards enemyCards;
    public int currentTurn = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        // player turn to enemy turn
        if(currentTurn == 1)
        {
            if(playerCards.handPoints <= enemyCards.handPoints)
            {
                Debug.Log("Game Over");
            }
            else
            {
                currentTurn = 2;
                enemyCards.SearchBeatableCard();
            }
                
        }
        // enemy turn to player turn
        else
        {
            if (enemyCards.handPoints <= playerCards.handPoints)
            {
                Debug.Log("You Win");
            }
            else
                currentTurn = 1;
        }
    }
}

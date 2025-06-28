using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardType = 0;    // 0 -normal, 1 - bolt, 2 - mirror
    public int points = 1;
    public bool faceDown = false;
    public Transform targetPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, targetPos.position) > 0.001f)
        {

        }
    }
}

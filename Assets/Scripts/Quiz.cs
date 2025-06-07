using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    public int category = 0; // anime/manga, games, tokusatsu, j-pop, hollywood
    public int difficulty = 0; // 0: easy, 1: medium, 2: hard
    public string question = "question?";
    public string rightAns = "right";
    public string[] wrongAns = new string[3] { "wrong", "wrong", "wrong" };
    public int rightAnsNo = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

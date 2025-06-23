using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapWall : MonoBehaviour
{
    int point = 1;
    private int maxPoint = 5;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.tag == "Racer")
        // {
        //     int racerPoint = other.GetComponent<RacerPlacement>().pointsPassed;
        //     if (racerPoint == point - 1)
        //     {
        //         racerPoint = point;
        //         Debug.Log(other.gameObject.name + " " + other.GetComponent<RacerPlacement>().lapsPassed);
        //     }

        // }
    }
}

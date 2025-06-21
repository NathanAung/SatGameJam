using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapWall : MonoBehaviour
{
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
        if(other.gameObject.tag == "Racer")
        {
            other.GetComponent<RacerPlacement>().lapsPassed += 1;
            Debug.Log(other.gameObject.name + " " + other.GetComponent<RacerPlacement>().lapsPassed);
        }
    }
}

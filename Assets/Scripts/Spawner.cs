using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] blockList;
    public bool spawning = true;


    // Start is called before the first frame update
    void Start()
    {
        //SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBlock()
    {
        if (!spawning) return;
        
        Instantiate(blockList[Random.Range(0, blockList.Length)], transform.position, Quaternion.identity);
    }
}

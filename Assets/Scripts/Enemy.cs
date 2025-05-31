using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool enemyActive = false;
    public int hp = 1;
    [SerializeField] private float speed = 3.5f;
    private float baseDistance;
    private float baseReachDist = 4f;
    NavMeshAgent agent;
    [SerializeField] Vector3 destinationPos;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        
        destinationPos = Base.basePos;

        agent.SetDestination(destinationPos);

        enemyActive = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (!enemyActive) return;

        baseDistance = Vector3.Distance(transform.position, destinationPos);
        if (baseDistance <= baseReachDist)
        {
            Debug.Log("Base reached");
            Destroy(this.gameObject);
        }
    }
}

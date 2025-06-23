using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RaceManager : MonoBehaviour
{
    public bool gameStarted = false;
    [SerializeField] GameObject startText;
    public List<RacerPlacement> racers = new List<RacerPlacement>();

    void Start()
    {

    }
    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
            gameStarted = true;
        }
        racers = racers.OrderByDescending(r => r.currentLap).ThenByDescending(r => r.currentWaypoint).ThenBy(r => r.distanceToNext).ToList();
    }

    public int GetPlayerPos()
    {
        for (int i = 0; i < racers.Count; i++)
        {
            if (racers[i].name == "Player")
            {
                return i + 1;
            }
        }

        return 5;
    }

    public void StartGame()
    {
        startText.SetActive(false);
        for (int i = 0; i < racers.Count; i++)
        {
            if (racers[i].name == "Player")
            {
                racers[i].GetComponent<RacerPlayer>().racerActive = true;
            }
            else
            {
                racers[i].GetComponent<RacerAI>().racerActive = true;
            }
        }
    }


}

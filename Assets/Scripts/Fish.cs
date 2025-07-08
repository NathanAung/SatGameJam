using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour {
    public int fishSize = 0; // 0 - small, 1 - medium, 2 - large
    public int fishID = 0; // 3 each
    private GameObject model;
    private float[] scales = { 30f, 60f, 100f };
    public enum States {
        swim,
        lured,
        biting,
        caught,
        ran
    }
    public States state = States.swim;
    [SerializeField] float swimSpeed = 8f;
    private float[] speeds = { 8f, 6f, 3f };
    private float pathFindTime = 5f;
    private float pathFindTimer = 0f;
    private Vector2 minPathPos = new Vector2(-4f, -7f); // x and z
    private Vector2 maxPathPos = new Vector2(4f, -1f); // x and z
    private Vector3 targetPos;
    private float biteTime = 3f;    // for time before biting
    private float biteTimer = 0f;
    private float hookedTime = 1f;  // QTE time when biting
    private float hookedTimer = 0f;
    public FishingArea fishingArea;


    void Awake() {
        model = transform.GetChild(0).gameObject;
    }


    // Start is called before the first frame update
    void Start() {
        SwimRandom();
        //SetUpFish(fishSize);
    }


    // Update is called once per frame
    void Update() {
        if (state == States.swim) {
            if (Vector3.Distance(transform.position, targetPos) > 0.1f) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, swimSpeed * Time.deltaTime);
            }
            else {
                if (pathFindTimer < pathFindTime) {
                    pathFindTimer += Time.deltaTime;
                }
                else {
                    FindPath();
                    pathFindTimer = 0f;
                }
            }
        }
        else if (state == States.lured) {
            if (Vector3.Distance(transform.position, targetPos) > 1f) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, swimSpeed * Time.deltaTime);
            }
            else {
                if (biteTimer < biteTime) {
                    biteTimer += Time.deltaTime;
                }
                else {
                    BiteBait();
                    biteTimer = 0f;
                }
            }
        }
        else if (state == States.biting) {
            if (Vector3.Distance(transform.position, targetPos) > 0.01f) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, swimSpeed * Time.deltaTime);
            }
            else {
                if (hookedTimer < hookedTime) {
                    hookedTimer += Time.deltaTime;
                }
                else {
                    RunAway();
                    hookedTimer = 0f;
                }
            }

        }
        else if (state == States.ran) {
            if (Vector3.Distance(transform.position, targetPos) > 0.1f) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, swimSpeed * Time.deltaTime);
            }
            else {
                Destroy(this.gameObject);
            }
        }
    }


    public void FindPath() {
        targetPos = new Vector3(Random.Range(minPathPos.x, maxPathPos.x), 0, Random.Range(minPathPos.y, maxPathPos.y));
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        model.transform.rotation = lookRotation;
    }


    public void SetUpFish(int size) {
        fishSize = size;
        model.transform.GetChild(0).transform.localScale = new Vector3(scales[size], scales[size], scales[size]);
        swimSpeed = speeds[size];
        switch (fishSize) {
            case 0:
                fishID = Random.Range(0, 3);
                biteTime = Random.Range(2f, 3f);
                break;
            case 1:
                fishID = Random.Range(3, 6);
                biteTime = Random.Range(3f, 4f);
                break;
            case 2:
                fishID = Random.Range(6, 9);
                biteTime = Random.Range(3f, 6f);
                break;
        }
    }


    public void SwimRandom() {
        Debug.Log("swimming");
        ResetVariables();
        FindPath();
        pathFindTime = Random.Range(2f, 6f);

        state = States.swim;
    }


    public void GetBaited(Vector3 hookPos) {
        Debug.Log("lured");
        ResetVariables();
        //targetPos = new Vector3(hookPos.x, 0, hookPos.z);
        targetPos = hookPos;
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        model.transform.rotation = lookRotation;

        state = States.lured;
    }


    private void BiteBait() {
        Debug.Log("biting");
        ResetVariables();

        state = States.biting;
    }


    public void RunAway() {
        Debug.Log("ran");
        ResetVariables();
        FindPath();
        fishingArea.CancelReel();

        state = States.ran;
    }


    public void GetCaught() {
        Debug.Log("caught");
        fishingArea.CatchFish(fishID);

        state = States.caught;

        Destroy(this.gameObject);
    }


    private void ResetVariables() {
        pathFindTimer = 0f;
        biteTimer = 0f;
        hookedTimer = 0f;
    }



}

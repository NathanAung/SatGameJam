using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingArea : MonoBehaviour {
    [SerializeField] GameObject[] fishModels;
    [SerializeField] GameObject fishPrefab;
    private GameObject fishParent;
    public GameObject closestFish;
    [SerializeField] int maxFishes = 5;
    GameObject target;
    [SerializeField] Rod rod;
    [SerializeField] LayerMask clickableLayer;
    bool casting = false;
    public bool targetValid = false;
    private float spawnCheckTime = 10f;
    private float spawnCheckTimer = 0f;
    private bool caught = false;
    private GameObject caughtFish;


    void Awake() {
        target = transform.GetChild(0).gameObject;
        fishParent = transform.GetChild(1).gameObject;
    }


    // Start is called before the first frame update
    void Start() {
        SpawnFishes();
    }


    // Update is called once per frame
    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer)) {
            //Debug.Log("Clicked on: " + hit.point);
            if (!casting)
                target.SetActive(true);

            target.transform.position = hit.point;
            targetValid = true;

            if (Input.GetMouseButtonDown(0)) {
                if (caught && !rod.reeling) {
                    if (caughtFish != null)
                        Destroy(caughtFish);
                    caughtFish = null;
                    caught = false;
                }
                else if (!rod.casting && !rod.reeling) {
                    rod.CastLine(hit.point);
                    casting = true;
                    target.SetActive(false);

                    // get closest fish to the casted position
                    closestFish = GetClosestFish(hit.point);
                    if (closestFish != null) {
                        Debug.Log("Closest fish is: " + closestFish.name);
                        closestFish.GetComponent<Fish>().GetBaited(hit.point);
                    }
                    else {
                        rod.Reel();
                    }
                }
                else if (rod.casted) {
                    if (closestFish != null) {
                        // if fish is biting
                        if (closestFish.GetComponent<Fish>().state == Fish.States.biting) {
                            // catch
                            closestFish.GetComponent<Fish>().GetCaught();
                        }
                        else {
                            // run away
                            closestFish.GetComponent<Fish>().RunAway();
                        }
                    }
                    rod.Reel();
                    casting = false;
                }
            }

            if (spawnCheckTimer < spawnCheckTime) {
                spawnCheckTimer += Time.deltaTime;
            }
            else {
                SpawnFishes();
                spawnCheckTimer = 0f;
            }

        }
        else {
            target.SetActive(false);
            targetValid = false;
        }
    }


    public void SpawnFishes() {
        if (fishParent.transform.childCount >= maxFishes) return;

        int spawns = maxFishes - fishParent.transform.childCount;
        for (int i = 0; i < spawns; i++) {
            Vector3 spawnPos = new Vector3(Random.Range(-4, 4), 0, Random.Range(-7, -1));
            GameObject fish = Instantiate(fishPrefab, spawnPos, Quaternion.identity, fishParent.transform);
            fish.GetComponent<Fish>().fishingArea = this;

            if (Random.Range(0, 7) == 0) {
                fish.GetComponent<Fish>().SetUpFish(2);
            }
            else if (Random.Range(0, 3) == 0) {
                fish.GetComponent<Fish>().SetUpFish(1);
            }
            else {
                fish.GetComponent<Fish>().SetUpFish(0);
            }
        }
    }


    public GameObject GetClosestFish(Vector3 position) {
        float closestDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (Transform fish in fishParent.transform) {
            float distance = Vector3.Distance(position, fish.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = fish.gameObject;
            }
        }

        return closest;
    }


    public void CatchFish(int id) {
        closestFish = null;
        caughtFish = Instantiate(fishModels[id], rod.hook.transform);
        caught = true;
    }


    public void CancelReel() {
        rod.Reel();
        closestFish = null;
    }
}

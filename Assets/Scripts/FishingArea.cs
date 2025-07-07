using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingArea : MonoBehaviour {
    GameObject target;
    [SerializeField] Rod rod;
    [SerializeField] LayerMask clickableLayer;
    bool casting = false;
    public bool targetValid = false;

    void Awake() {
        target = transform.GetChild(0).gameObject;
    }
    // Start is called before the first frame update
    void Start() {

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
                if (!rod.casting && !rod.reeling) {
                    rod.CastLine(hit.point);
                    casting = true;
                    target.SetActive(false);
                }
                else if (rod.casted) {
                    rod.Reel();
                    casting = false;
                }
            }

        }
        else {
            target.SetActive(false);
            targetValid = false;
        }
    }
}

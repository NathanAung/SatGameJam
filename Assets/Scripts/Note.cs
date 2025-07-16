using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Note : MonoBehaviour {
    [SerializeField] private float noteSpeed = 0.5f;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float xScale = transform.localScale.x - noteSpeed * Time.deltaTime;
        float yScale = transform.localScale.y - noteSpeed * Time.deltaTime;
        float zScale = transform.localScale.z - noteSpeed * Time.deltaTime;
        transform.localScale = new Vector3(xScale, yScale, zScale);


        if (transform.localScale.x <= 0.3f) {
            Debug.Log("MISS");
            Destroy(this.gameObject);
        }
    }
}

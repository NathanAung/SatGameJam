using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private float noteSpeed = 0.5f;
    [SerializeField] private float perfectScaleMin = 0.5f;
    [SerializeField] private float perfectScaleMax = 0.53f;
    [SerializeField] private float goodScaleMin = 0.46f;
    [SerializeField] private float goodScaleMax = 0.6f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        float xScale = transform.localScale.x - noteSpeed * Time.deltaTime;
        float yScale = transform.localScale.y - noteSpeed * Time.deltaTime;
        float zScale = transform.localScale.z - noteSpeed * Time.deltaTime;
        transform.localScale = new Vector3(xScale, yScale, zScale);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (transform.localScale.x >= perfectScaleMin && transform.localScale.x <= perfectScaleMax) {
                Debug.Log("PERFECT");
            }
            else if (transform.localScale.x >= goodScaleMin && transform.localScale.x <= goodScaleMax) {
                Debug.Log("GOOD");
            }
            else {
                Debug.Log("BAD");
            }
            Destroy(this.gameObject);
        }
        
        if (transform.localScale.x <= 0.3f) {
            Debug.Log("MISS");
            Destroy(this.gameObject);
        }
    }
}

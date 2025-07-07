using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour {
    private Transform castPoint;
    private GameObject hook;
    private LineRenderer line;
    public bool casting = false;
    public bool casted = false;
    public bool reeling = false;
    public Vector3 castPos;
    float castSpeed = 5f;


    void Awake() {
        castPoint = transform.GetChild(0).transform;
        hook = transform.GetChild(1).gameObject;
        castPos = castPoint.position;
        line = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start() {
        line.positionCount = 2;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.white;
        line.endColor = Color.white;

    }

    // Update is called once per frame
    void Update() {
        line.SetPosition(0, castPoint.position);
        line.SetPosition(1, hook.transform.position);

        if (Vector3.Distance(hook.transform.position, castPos) > 0.01f) {
            hook.transform.position = Vector3.MoveTowards(hook.transform.position, castPos, castSpeed * Time.deltaTime);
        }
        else if (casting && !casted) {
            casted = true;
        }
        else if (reeling)
            reeling = false;
    }

    public void CastLine(Vector3 pos) {
        castPos = pos;
        casting = true;
    }

    public void Reel() {
        castPos = castPoint.position;
        casting = false;
        casted = false;
        reeling = true;
    }
}

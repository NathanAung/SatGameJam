using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ReceiveDamage(int dmg, Vector2 knockback) {
        transform.parent.GetComponent<Character>().ReceiveDamage(dmg, knockback);
    }
}

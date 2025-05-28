using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    [SerializeField] int damage = 1;
    [SerializeField] Vector2 knockback = Vector2.zero;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("collided");
        Character target = collision.transform.parent.GetComponent<Character>();

        Debug.Log(collision.gameObject.name);
        if (target != null) {
            // flip knockback direction
            Vector2 knockbackDir = transform.parent.localScale.x > 0 ? knockback : new Vector2(knockback.x * -1, knockback.y);
            target.ReceiveDamage(damage, knockbackDir);
            //Debug.Log("Dealt " + damage + " to " + collision.name);
        }
    }
}

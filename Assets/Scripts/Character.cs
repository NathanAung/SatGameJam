using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    protected Rigidbody2D rb;
    public int hp = 3;
    [SerializeField] protected int maxHp = 3;
    protected bool wasHit = false;
    protected float resetHitTime = 0.5f;

    protected bool canMove = true;


    // Start is called before the first frame update
    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {

    }


    public virtual void ReceiveDamage(int dmg, Vector2 knockback) {
        hp = Mathf.Max(0, hp - dmg);
        rb.velocity = knockback;
        Debug.Log(knockback);
        if (hp <= 0) {
            Debug.Log("Game Over");
        }
        wasHit = true;
        Invoke(nameof(ResetHit), resetHitTime);
    }

    protected void ResetHit() {
        wasHit = false;
    }
}

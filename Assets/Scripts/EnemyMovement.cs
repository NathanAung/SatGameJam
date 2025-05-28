using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Character {

    // movement
    private float moveSpeed = 0f;
    [SerializeField] float moveSpeedDefault = 1f;
    private Vector2 moveDirection = Vector2.right;
    private int facingDir = 1;
    public LayerMask groundLayer;
    [SerializeField] bool grounded = false;
    [SerializeField] bool hitGround = false;
    private float height = 2f;
    bool notOnEdge = false;
    [SerializeField] Transform edgeCheckPos;

    // attack
    public LayerMask playerLayer;
    private bool attacking = false;
    private bool playerInRange = false;
    [SerializeField] private float attackCD = 2f;
    private float attackTimer = 0f;
    [SerializeField] Collider2D hitbox;
    private float hitboxActiveTime = 0.2f;


    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        moveSpeed = moveSpeedDefault;
    }

    // Update is called once per frame
    void Update() {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, height * 0.7f, groundLayer);
        notOnEdge = Physics2D.Raycast(edgeCheckPos.position, Vector2.down, height * 0.7f, groundLayer);
        playerInRange = Physics2D.Raycast(transform.position, moveDirection, 1f, playerLayer);

        if (playerInRange) {
            if (attackTimer < attackCD) {
                attackTimer += Time.deltaTime;
            }
            else {
                Attack();
                attackTimer = 0;
            }
        }
    }

    void FixedUpdate() {
        if (grounded && canMove && !playerInRange) {
            Move();
            SpeedControl();
        }
    }

    private void Move() {
        if (grounded && !notOnEdge) {
            if (moveDirection == Vector2.left) moveDirection = Vector2.right;
            else moveDirection = Vector2.left;
        }

        transform.localScale = new Vector3(moveDirection.x, 1, 1);
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f);
    }

    private void SpeedControl() {
        Vector2 flatVel = new Vector2(rb.velocity.x, 0f);
        if (flatVel.magnitude > moveSpeed) {
            Vector2 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector2(limitedVel.x, rb.velocity.y);
        }
    }

    private void Attack() {
        Debug.Log("Enemy Attacking");
        hitbox.enabled = true;
        Invoke(nameof(DisableAttack), hitboxActiveTime);
    }

    private void DisableAttack() {
        hitbox.enabled = false;
    }


}

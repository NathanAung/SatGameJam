using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Character {

    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] float moveSpeedDefault = 10f;
    [SerializeField] float moveSpeedDash = 100f;
    private float horizontalInput;
    private float verticalInput;
    private Vector2 moveDirection;

    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpCooldown = 0.25f;
    [SerializeField] float airMultiplier = 0.4f;
    private bool canJump = true;

    private bool isDashing = false;

    public float playerHeight = 2f;
    public LayerMask groundLayer;
    [SerializeField] bool grounded = false;
    [SerializeField] bool hitGround = false;

    // ATTACKS
    private bool isAttacking = false;
    private bool canAttack = true;
    private bool canCombo = false;
    private float comboWindow = 1f;
    private float comboTimer = 0f;
    private int currentAttack = 0;
    private int lastAttack = 0;
    private int maxAttack = 0;
    private float hitboxActiveTime = 0.2f;
    private float hitboxActiveTimer = 0f;
    [SerializeField] GameObject[] hitboxes;


    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        maxAttack = hitboxes.Length;
    }

    // Update is called once per frame
    void Update() {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight * 0.7f, groundLayer);

        if (grounded && !hitGround && canJump) {
            hitGround = true;
            if (!canJump) canJump = true;
        }

        if (canCombo) {
            if (comboTimer < comboWindow) {
                comboTimer += Time.deltaTime;
            }
            else {
                comboTimer = 0f;
                currentAttack = 0;
                canCombo = false;
                Debug.Log("Combo timeout");
            }
        }



        PlayerInput();
    }


    private void FixedUpdate() {
        if (!isDashing) {
            Move();
            SpeedControl();
        }
    }


    private void PlayerInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");
        if (!isDashing) {
            moveSpeed = (horizontalInput != 0 || verticalInput != 0) ? moveSpeedDefault : 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump && grounded) {
            Jump();
        }

        if (Input.GetButtonDown("Fire1") && canAttack) {
            Attack();

        }
    }


    private void Move() {
        if (horizontalInput != 0) {
            moveDirection.x = horizontalInput;
            transform.localScale = new Vector3(horizontalInput, 1, 1);
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f);
        }

    }


    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f);
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        hitGround = false;
        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);
    }


    private void ResetJump() {
        canJump = true;
    }


    private void SpeedControl() {
        if (wasHit) return;

        Vector2 flatVel = new Vector2(rb.velocity.x, 0f);
        if (flatVel.magnitude > moveSpeed) {
            Vector2 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector2(limitedVel.x, rb.velocity.y);
        }
    }

    private void Attack() {
        EnableAttack();

        canAttack = false;
    }


    private void EnableAttack() {
        if (hitboxes[lastAttack].activeSelf) {
            hitboxes[lastAttack].SetActive(false);

        }

        hitboxes[currentAttack].SetActive(true);
        Debug.Log(currentAttack);
        canCombo = true;
        comboTimer = 0f;
        lastAttack = currentAttack;
        currentAttack += 1;
        if (currentAttack >= maxAttack) {
            currentAttack = 0;
            canCombo = false;
        }

        Invoke(nameof(DisableAttack), hitboxActiveTime);
        //Invoke(nameof(ComboTimeout), comboWindow);

    }


    private void DisableAttack() {
        hitboxes[lastAttack].SetActive(false);
        canAttack = true;
        Debug.Log("disabled");
    }

    private void ComboTimeout() {
        canCombo = false;
        currentAttack = 0;
        Debug.Log("combo timeout");
    }
}

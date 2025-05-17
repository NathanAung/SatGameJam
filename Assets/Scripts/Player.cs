using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject playerBullet;
    [SerializeField] Transform shootPos;
    int maxHP = 10;
    int hp = 10;

    float moveSpeed = 7f;
    Vector2 moveDir = Vector2.zero;
    // movement boundary
    Vector2 minBounds = new Vector2(-8.5f, -4.4f);
    Vector2 maxBounds = new Vector2(-3.6f, 4.4f);

    float shootInterval = 0.2f;
    float prevTime = 0f;



    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
        if (moveDir.x != 0 || moveDir.y != 0)
        {
            //float newX = Mathf.Clamp(transform.position.x + moveSpeed * horizontalInput * Time.deltaTime, boundXMin, boundXMax);
            //float newY = Mathf.Clamp(transform.position.y + moveSpeed * verticalInput * Time.deltaTime, boundYMin, boundYMax);
            //transform.position = new Vector2(newX, newY);

            Vector3 move = (Vector3)moveDir.normalized * moveSpeed * Time.deltaTime;
            Vector3 clamped = transform.position + move;
            clamped.x = Mathf.Clamp(clamped.x, minBounds.x, maxBounds.x);
            clamped.y = Mathf.Clamp(clamped.y, minBounds.y, maxBounds.y);

            transform.position = clamped;
        }

        // shoot
        if (Input.GetKey(KeyCode.Space) && Time.time - prevTime > shootInterval)
        {
            GameObject bullet = Instantiate(playerBullet, shootPos.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().ActivateBullet(0);

            prevTime = Time.time;
        }
    }

    public void ReceiveDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log("Player took damage, HP: " + hp);

        if (hp <= 0)
        {
            Debug.Log("Game Over");
            Destroy(this.gameObject);
        }
    }
}

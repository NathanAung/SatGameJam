using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // movement boundary
    Vector2 minBounds = new Vector2(-10f, -6f);
    Vector2 maxBounds = new Vector2(10f, 6f);

    public Vector2 moveDir = Vector2.zero;
    float moveSpeed = 5f;
    int damage = 1;
    public bool bulletActive = false;
    int bulletType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OutOfBounds())
            Destroy(this.gameObject);


        if (bulletActive)
        {
            // change directions according to bullet type
            Vector3 move = (Vector3)moveDir.normalized * moveSpeed * Time.deltaTime;
            transform.position += move;
        }
        
    }

    public void ActivateBullet(int type)
    {
        bulletType = type;
        switch (bulletType)
        {
            // player bullet
            case 0:
                moveDir = Vector2.right;
                moveSpeed = 10f;
                GetComponent<CircleCollider2D>().excludeLayers = LayerMask.GetMask("Player", "Bullet");
                break;
            // enemy bullet 1
            case 1:
                moveDir = Vector2.left;
                moveSpeed = 7f;
                GetComponent<CircleCollider2D>().excludeLayers = LayerMask.GetMask("Enemy", "Bullet");
                break;
        }
        bulletActive = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bulletActive) return;

        if (bulletType == 0 && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().ReceiveDamage(damage);
            Destroy(this.gameObject);

        }
        if (bulletType == 1 && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().ReceiveDamage(damage);
            Destroy(this.gameObject);

        }

    }

    private bool OutOfBounds()
    {
        if (transform.position.x > maxBounds.x || transform.position.x < minBounds.x || transform.position.y > maxBounds.y || transform.position.y < minBounds.y)
            return true;

        return false;
    }
}

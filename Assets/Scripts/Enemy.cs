using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject enemyBullet;
    [SerializeField] Transform shootPos;
    public EnemySpawner spawner;
    float minY = -4.4f;
    float maxY = 4.4f;
    public int enemyType = 0;

    [SerializeField] protected int maxHP = 1;
    protected int hp = 1;

    float shootTime = 1f;
    float shootTimer = 0f;

    float moveSpeed = 4f;
    public Vector2 moveDir = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
        //moveDir = Vector2.down;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 move = (Vector3)moveDir.normalized * moveSpeed * Time.deltaTime;
        transform.position += move;
        if(moveDir == Vector2.up && transform.position.y >= maxY)
        {
            moveDir = Vector2.down;
        }
        else if (moveDir == Vector2.down && transform.position.y <= minY)
        {
            moveDir = Vector2.up;
        }

        if (shootTimer < shootTime)
        {
            shootTimer += Time.deltaTime;
        }
        else
        {
            Shoot();
            shootTimer = 0f;
        }
    }


    public void ReceiveDamage(int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            Debug.Log("Enemy dead");
            spawner.activeEnemies -= 1;
            Destroy(this.gameObject);
        }
    }

    protected void Shoot()
    {
        GameObject bullet = Instantiate(enemyBullet, shootPos.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().ActivateBullet(1);
    }
}

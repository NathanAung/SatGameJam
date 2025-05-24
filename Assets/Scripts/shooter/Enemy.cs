using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject enemyBullet;
    [SerializeField] protected Transform shootPos;
    public EnemySpawner spawner;
    protected float minY = -4.4f;
    protected float maxY = 4.4f;
    public int enemyType = 0;

    [SerializeField] protected int maxHP = 1;
    protected int hp = 1;
    [SerializeField] protected int score = 10;

    [SerializeField] protected float shootTime = 1f;
    protected float shootTimer = 0f;

    [SerializeField] protected float moveSpeed = 7f;
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
        Move();

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
        if (hp <= 0)
        {
            Debug.Log("Enemy dead");
            spawner.activeEnemies -= 1;
            GameManager.UpdateScore(score);
            Destroy(this.gameObject);
        }
    }

    protected virtual void Shoot()
    {
        GameObject bullet = Instantiate(enemyBullet, shootPos.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().ActivateBullet(1, Vector2.zero);
    }

    protected virtual void Move()
    {
        Vector3 move = (Vector3)moveDir.normalized * moveSpeed * Time.deltaTime;
        transform.position += move;
        if (moveDir == Vector2.up && transform.position.y >= maxY)
        {
            moveDir = Vector2.down;
        }
        else if (moveDir == Vector2.down && transform.position.y <= minY)
        {
            moveDir = Vector2.up;
        }
    }
}

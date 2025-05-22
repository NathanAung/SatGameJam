using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRing : Enemy
{
    protected float minX = 4f;
    protected float maxX = 14f;


    // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    void Update()
    {
        Move();

        // if (shootTimer < shootTime)
        // {
        //     shootTimer += Time.deltaTime;
        // }
        // else
        // {
        //     Shoot();
        //     shootTimer = 0f;
        // }
    }

    protected override void Shoot()
    {
        //base.Shoot();

        //Vector2[] directions = new Vector2[12];
        for (int i = 0; i < 12; i++)
        {
            float angle = i * 30f * Mathf.Deg2Rad; // Convert degrees to radians
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            GameObject bullet = Instantiate(enemyBullet, shootPos.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().ActivateBullet(2, dir);
        }
    }

    protected override void Move()
    {
        Vector3 move = (Vector3)moveDir.normalized * moveSpeed * Time.deltaTime;
        transform.position += move;
        if (moveDir == Vector2.right && transform.position.x >= maxX)
        {
            //Shoot();
            moveDir = Vector2.left;
        }
        else if (moveDir == Vector2.left && transform.position.x <= minX)
        {
            Shoot();

            moveDir = Vector2.right;
        }
    }
}

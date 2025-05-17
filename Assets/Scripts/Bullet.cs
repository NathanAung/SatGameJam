using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 moveDir = Vector2.zero;
    float moveSpeed = 5f;
    public bool bulletActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletActive)
        {
            // change directions according to bullet type
            Vector3 move = (Vector3)moveDir.normalized * moveSpeed * Time.deltaTime;
            transform.position += move;
        }
        
    }

    public void ActivateBullet(int type)
    {
        switch (type)
        {
            // player bullet
            case 0:
                moveDir = Vector2.right;
                break;
        }
        bulletActive = true;
    }
}

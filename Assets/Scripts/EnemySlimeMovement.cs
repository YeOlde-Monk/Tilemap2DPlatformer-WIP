using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlimeMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;

    Rigidbody2D slimeRb2d;
    
    void Start()
    {
        slimeRb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        slimeRb2d.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(slimeRb2d.velocity.x)), 1f);
    }
}

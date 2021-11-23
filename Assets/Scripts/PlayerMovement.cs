using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float runSpeed = 2f;
    [SerializeField] float climbSpeed = 2f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 5f;

    [Header("Climbing")]
    [SerializeField] float climbAnimationSpeed = 1f;
    
    [Header("Melee Attack")]
    [SerializeField] Vector2 knockBack = new Vector2 (10f, 5f);
    
    [Header("Range Attack")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D playerRb2d;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float usualGravity = 3f;

    bool isAlive = true;

    void Start()
    {
        playerRb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        playerRb2d.gravityScale = usualGravity;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }
    
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            playerRb2d.velocity += new Vector2(0f, jumpForce);
            
            playerAnimator.SetTrigger("doJump");
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRb2d.velocity.y);
        playerRb2d.velocity = playerVelocity;

        bool playerMoves = Mathf.Abs(playerRb2d.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerMoves);
    }

    void FlipSprite()
    {
        bool movesHorizontally = Mathf.Abs(playerRb2d.velocity.x) > Mathf.Epsilon;

        if (movesHorizontally && !playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRb2d.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        bool movesVertically = Mathf.Abs(playerRb2d.velocity.y) > Mathf.Epsilon;

        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Vector2 climbVelocity = new Vector2(playerRb2d.velocity.x, moveInput.y * climbSpeed);
            playerRb2d.velocity = climbVelocity;
            playerRb2d.gravityScale = 0f;
        

            if (movesVertically)
            {
                playerAnimator.SetBool("isClimbing", movesVertically);
                playerAnimator.SetFloat("climbingSpeed", climbAnimationSpeed * Mathf.Sign(playerRb2d.velocity.y));
            }

            if (!movesVertically)
            {
                playerAnimator.SetFloat("climbingSpeed", 0f);
            }
        }
        else
        {
            playerAnimator.SetBool("isClimbing", false);
            playerRb2d.gravityScale = usualGravity;
        }
    }
    
    void Die()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Obstacles")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("hasDied");
            playerRb2d.velocity = knockBack;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}

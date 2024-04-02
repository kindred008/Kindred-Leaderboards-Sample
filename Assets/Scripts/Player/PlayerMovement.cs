using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 5;

    private PlayerInput playerInput;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSprite;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        var moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        if (moveInput.x > 0)
        {
            playerSprite.flipX = false;
        } else if (moveInput.x < 0)
        {
            playerSprite.flipX= true;
        }

        playerRb.velocity = new Vector2(moveInput.x * moveSpeed, playerRb.velocity.y);
    }
}

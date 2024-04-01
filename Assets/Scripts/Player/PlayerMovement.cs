using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 5;

    private PlayerInput playerInput;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        
        playerRb.velocity = new Vector2(moveInput.x * moveSpeed, playerRb.velocity.y);
    }
}

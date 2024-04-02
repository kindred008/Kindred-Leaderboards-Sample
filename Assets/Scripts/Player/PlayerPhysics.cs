using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private int bounceStrength = 8;

    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        var halfSize = (playerCollider.size.y / 2) * transform.localScale.y;
        var offset = new Vector3(halfSize, 0);

        if (Physics2D.Raycast(transform.position, Vector2.down, halfSize + 0.05f) || 
            Physics2D.Raycast(transform.position + offset, Vector2.down, halfSize + 0.05f) || 
            Physics2D.Raycast(transform.position - offset, Vector2.down, halfSize + 0.05f))
        {
            if (playerRb.velocity.y <= 0)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceStrength);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        RaycastHit2D leftHit = Physics2D.Raycast(transform.position - offset, Vector2.down, halfSize + 0.05f);
        RaycastHit2D middleHit = Physics2D.Raycast(transform.position, Vector2.down, halfSize + 0.05f);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + offset, Vector2.down, halfSize + 0.05f);

        var raycastHitArray = new RaycastHit2D[] {leftHit, middleHit, rightHit};
        var raycastHitArrayCollided = raycastHitArray.Where(x => x.collider != null).ToArray();

        if (raycastHitArrayCollided.Length > 0)
        {
            if (playerRb.velocity.y <= 0)
            {
                foreach (var ray in raycastHitArrayCollided)
                {
                    if (ray.collider.CompareTag("GroundTrap"))
                    {
                        GameManager.OnGameOver.Invoke();
                    }
                }

                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceStrength);

                var destructableHit = raycastHitArrayCollided.FirstOrDefault(x => x.collider.GetComponent<Destructable>() != null);
                if (!destructableHit.Equals(default(RaycastHit2D)))
                {
                    destructableHit.collider.GetComponent<Destructable>().Damage();
                }
            }
        }
    }
}

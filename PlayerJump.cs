using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private PlayerMovementState playerMovementState;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem doubleJumpParticles;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float doubleJumpForce = 6f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(4f, 8f);
    [SerializeField] private float wallJumpMovementCooldown = 0.2f;
    private PlayerMovement playerMovement;
    private float playerHalfHeight;
    private float playerHalfWidth;
    private bool canDoubleJump;

    private void Start()
    {
        playerHalfWidth = spriteRenderer.bounds.extents.x;
        playerHalfHeight = spriteRenderer.bounds.extents.y;
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            CheckJumpType();
        }
    }

    private void CheckJumpType()
    {
        bool isGrounded = GetIsGrounded();

        if (isGrounded)
        {
            playerMovementState.SetMoveState(PlayerMovementState.MoveState.Jump);
            Jump(jumpForce);
        }
        else
        {
            // Do we do a double jump or do we do a wall jump?
            int direction = GetWallJumpDirection();
            if (direction == 0 && canDoubleJump && rigidBody.velocity.y <= 4f)
            {
                // We are not standing next to a wall and we should do a double jump
                DoubleJump();
            }
            else if (direction != 0)
            {
                // Direction is -1 or +1 and we should do a wall jump
                WallJump(direction);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GetIsGrounded();
    }

    private int GetWallJumpDirection()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, playerHalfWidth + 0.1f, LayerMask.GetMask("Ground")))
        {
            return -1;
        }
        if (Physics2D.Raycast(transform.position, Vector2.left, playerHalfWidth - 0.1f, LayerMask.GetMask("Ground")))
        {
            return 1;
        }

        return 0;
    }

    private bool GetIsGrounded()
    {
        bool hit = Physics2D.Raycast(transform.position, Vector2.down, playerHalfHeight + 0.1f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            canDoubleJump = true;
        }

        return hit;
    }

    private void DoubleJump()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        Jump(doubleJumpForce);
        canDoubleJump = false;
        doubleJumpParticles.Play();
        playerMovementState.SetMoveState(PlayerMovementState.MoveState.Double_Jump);
    }

    private void WallJump(int direction)
    {
        Vector2 force = wallJumpForce;
        force.x *= direction;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        playerMovement.wallJumpCooldown = wallJumpMovementCooldown;
        rigidBody.AddForce(force, ForceMode2D.Impulse);
        playerMovementState.SetMoveState(PlayerMovementState.MoveState.Wall_Jump);
    }

    private void Jump(float force)
    {
        // We are adding force to the already existing force
        rigidBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}

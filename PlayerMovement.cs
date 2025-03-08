using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;

    [Header("Input")]
    [SerializeField] private float speed = 5f;
    private Vector2 movement;
    private Vector2 screenBounds;
    private float playerHalfWidth;
    #endregion

    private void Start()
    {
        // We need to perform a calculation that converts screen width and screen height into world units
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        playerHalfWidth = spriteRenderer.bounds.extents.x;
        particlesStartPos = dustParticles.transform.localPosition;
    }

    void Update()
    {
        HandleMovement();
        ClampMovement();
        FlipCharacterX();
    }

    private void FlipCharacterX()
    {
        float input = Input.GetAxis("Horizontal");
        if (input > 0 && (transform.position.x > xPosLastFrame)){
            // We are moving right
            spriteRenderer.flipX = false;
        }

        else if (input < 0 && (transform.position.x < xPosLastFrame)){
            // We are moving left
            spriteRenderer.flipX = true;
        }
    }

    private void ClampMovement()
    {
        float clampedX = Mathf.Clamp(transform.position.x, -screenBounds.x + playerHalfWidth, screenBounds.x - playerHalfWidth);
        Vector2 pos = transform.position; // Get the player's current position
        pos.x = clampedX; // Reassign the X value to the clamped position
        transform.position = pos; // Reassign the clamped value back to the player
    }

    private void HandleMovement()
    {
        float input = Input.GetAxis("Horizontal");
        movement.x = input * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}

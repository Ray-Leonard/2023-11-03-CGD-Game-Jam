using UnityEngine;

public class Move2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get input from the player
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        // Create a movement vector
        Vector2 movement = new Vector2(moveX, moveY);
        // Normalize the vector to prevent diagonal movement from being faster
        movement.Normalize();
        // Apply the movement to the Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}


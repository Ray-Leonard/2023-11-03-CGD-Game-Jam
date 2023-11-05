using UnityEngine;

public class Move2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        movement *= moveSpeed * Time.deltaTime;
        // Apply the movement to the Rigidbody2D
        transform.position += new Vector3(movement.x, movement.y, 0);

        if(movement != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}


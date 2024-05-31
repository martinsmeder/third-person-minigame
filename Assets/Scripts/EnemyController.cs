using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 20f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get and store the enemy's Rigidbody component
    }

    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (player != null) // Check if the player reference is set
        {
            Vector3 direction = (player.position - transform.position).normalized; // Get direction
            rb.AddForce(direction * moveSpeed); // Apply force to move the enemy towards the player
        }
    }
}

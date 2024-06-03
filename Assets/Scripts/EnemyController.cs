using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 10f; 
    private float speedIncrement = 5f; 
    private float incrementInterval = 5f; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get and store the enemy's Rigidbody component
        StartCoroutine(IncreaseSpeedOverTime()); // Start the coroutine to increase speed over time
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

    private IEnumerator IncreaseSpeedOverTime()
    {
        while (true) // Infinite loop to keep the coroutine running
        {
            yield return new WaitForSeconds(incrementInterval); // Wait for the specified interval
            moveSpeed += speedIncrement; // Increase the movement speed
        }
    }
}

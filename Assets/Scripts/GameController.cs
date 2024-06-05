using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform player;
    public Transform enemyParent;

    private GameObject[] enemies;
    private int secondsCounter;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        StartCoroutine(CountSeconds()); 
    }

    void Update()
    {
        CheckCollisions();
        CheckOutOfBounds();
    }

    private void CheckCollisions()
    {
        foreach (GameObject enemy in enemies)
        {
            // Get distance between player and enemy
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < 1)
            {
                Debug.Log("Game over!");
            }
        }
    }

    private void CheckOutOfBounds()
    {
        if (player.position.y < 0) // Check if player is beneath the ground 
        {
            Debug.Log("Game over!");
        }
    }

    private IEnumerator CountSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // Wait for 1 second
            secondsCounter++; // Increment the counter
            Debug.Log("Time: " + secondsCounter);

            if (secondsCounter == 60)
            {
                Debug.Log("You beat the game!");
            }
        }
    }
}

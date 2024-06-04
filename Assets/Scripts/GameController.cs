using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform player;
    public Transform enemyParent;

    private GameObject[] enemies;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
            Debug.Log("Game over");
        }
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public Transform player;
    public Transform enemyParent;
    public TextMeshProUGUI countText;
    public GameObject looseTextObject;
    public GameObject winTextObject;

    private GameObject[] enemies;
    private int count;
    private bool gameOver;


    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        looseTextObject.SetActive(false);
        winTextObject.SetActive(false);
        
        StartCoroutine(CountSeconds());
        SetCountText();
    }

    void Update()
    {
        if (gameOver) return;

        CheckCollisions();
        CheckOutOfBounds();
    }

    void SetCountText()
    {
        countText.text = count.ToString();
    }

    private void CheckCollisions()
    {
        foreach (GameObject enemy in enemies)
        {
            // Get distance between player and enemy
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < 1)
            {
                GameOver(false);
                break;
            }
        }
    }

    private void CheckOutOfBounds()
    {
        if (player.position.y < 0) // Check if player is beneath the ground 
        {
            GameOver(false);
        }
    }

    private IEnumerator CountSeconds()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(1); // Wait for 1 second
            count++; // Increment the counter
            SetCountText();

            if (count == 90)
            {
                GameOver(true);
            }
        }
    }

    private void GameOver(bool won)
    {
        gameOver = true;
        StopCoroutine(CountSeconds());
        if (won)
        {
            winTextObject.SetActive(true);
        }
        else
        {
            looseTextObject.SetActive(true);
        }

        Time.timeScale = 0f; // Pause the game
    }
}

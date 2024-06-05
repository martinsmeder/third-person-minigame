using System.Collections;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public Transform player;
    public Transform enemyParent;
    public TextMeshProUGUI countText;
    public GameObject looseTextObject;

    private GameObject[] enemies;
    private int count;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        looseTextObject.SetActive(false);   
        StartCoroutine(CountSeconds());
        SetCountText();
    }

    void Update()
    {
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
                looseTextObject.SetActive(true);
            }
        }
    }

    private void CheckOutOfBounds()
    {
        if (player.position.y < 0) // Check if player is beneath the ground 
        {
            looseTextObject.SetActive(true);
        }
    }

    private IEnumerator CountSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // Wait for 1 second
            count++; // Increment the counter
            SetCountText();

            if (count == 60)
            {
                Debug.Log("You beat the game!");
            }
        }
    }
}

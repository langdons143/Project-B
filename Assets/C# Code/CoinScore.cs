// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=ES2jPGnWQoU

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinScore : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
    }
    private static int coinPointsCollected;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched the coin: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected coin!");

            ScoreManager.sManager.IncreaseScore(10);
            coinPointsCollected += 10;

            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.Pickup);
            }

            Debug.Log("Destroying coin: " + gameObject.name);

            Destroy(gameObject);

            CheckWinCondition();
        }
    }

    // 50 Score = Win Event
    private void CheckWinCondition()
    {
        if (coinPointsCollected >= 100)
        {
            // Trigger win event here
            Debug.Log("You Win!");
            audioManager.PlaySFX(audioManager.onWin);
            SceneManager.LoadScene(4);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
    }
    public static void ResetCoins()
    {
        coinPointsCollected = 0;
    }
}
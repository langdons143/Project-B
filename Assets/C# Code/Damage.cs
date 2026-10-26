// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=bRcMVkJS3XQ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float damage;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.instance;
    }

    private void Update()
    {
        // Don't check player death here anymore.
        // We'll handle death when the bullet hits.
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if bullet hit the player
        PlayerHealth hitPlayer =
            other.GetComponentInParent<PlayerHealth>();

        if (hitPlayer != null)
        {
            // Damage player
            hitPlayer.health -= damage;

            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.damage);
            }

            // Check if player died
            if (hitPlayer.health <= 0)
            {
                if (audioManager != null)
                {
                    audioManager.PlaySFX(audioManager.onDeath);
                }

                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().name);
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            // Destroy bullet when it hits a wall
            Destroy(gameObject);
        }
    }
}
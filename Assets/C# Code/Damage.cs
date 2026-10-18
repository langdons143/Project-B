// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=bRcMVkJS3XQ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    public PlayerHealth playerHealth ;
    public float damage;
    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.health <= 0)
        {
            Destroy(gameObject); // Destroy the enemy object when player health reaches 0
            audioManager.PlaySFX(audioManager.onDeath);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.health -= damage;
            audioManager.PlaySFX(audioManager.damage);
        }
    }

}

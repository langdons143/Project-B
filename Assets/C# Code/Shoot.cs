// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=p6GFROM1A8w

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Shoot : MonoBehaviour
{
    public GameObject bulletObject;
    public Transform player;
    public float shootingCooldown = 3f;
    public float shootSpeed = 40f;

    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
    }

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootingCooldown)
        {
            ShootPlayer();
            timer = 0f; // Reset the timer
        }
    }

    private void ShootPlayer()
    {
        // Calculate the direction from the shooter to the player
        Vector3 direction = (player.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletObject, transform.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * shootSpeed;

        // Play shooting sound
        audioManager.PlaySFX(audioManager.Shoot);

        // Destroy the bullet after it hits player
        Destroy(bullet, 4f);  // Adjust the time as needed
    }
}

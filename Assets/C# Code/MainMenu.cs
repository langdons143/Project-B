// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        audioManager.PlaySFX(audioManager.buttonClick);
    }

    public void Credits()
    {
        SceneManager.LoadScene(3);
        audioManager.PlaySFX(audioManager.buttonClick);
    }

    public void Options()
    {
        SceneManager.LoadScene(2);
        audioManager.PlaySFX(audioManager.buttonClick);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        audioManager.PlaySFX(audioManager.buttonClick);
    }
}

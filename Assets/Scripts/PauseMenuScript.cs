using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    public GameObject pauseMenu;

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        player.GetComponent<ThirdPersonController>().paused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        player.GetComponent<ThirdPersonController>().paused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
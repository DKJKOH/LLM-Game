using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject GPTChatMenu, ShopMenu, PauseMenu, NVG;

    public TextMeshProUGUI difficultyText;

    private bool isHard = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GPTChatMenu.activeSelf && !ShopMenu.activeSelf)
        {
            // Toggle pause menu on / off
            PauseMenu.SetActive(!PauseMenu.activeSelf);
        }

        // If pause menu is active
        if (PauseMenu.activeSelf)
        {
            // Continue time
            Time.timeScale = 0f;
        }
        else
        {
            // Stop Time
            Time.timeScale = 1f;
        }
    }

    public void closePauseMenu()
    {
        // Deactivate pause menu
        PauseMenu.SetActive(false);

        // Resume time
        Time.timeScale = 1f;
    }

    public void toggleDifficulty()
    {
        // Toggle NVG
        NVG.GetComponent<night_vision>().toggleOffOn();

        // Toggle between hard and easy
        isHard = !isHard;

        // Change button text depending if hard or easy
        if (isHard)
        {
            difficultyText.text = "Difficulty - Hard";
        }
        else
        {
            difficultyText.text = "Difficulty - Easy";
        }
    }

    // Return to main menu
    public void returnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Quit game
    public void quitGame()
    {
        Application.Quit();
    }

}

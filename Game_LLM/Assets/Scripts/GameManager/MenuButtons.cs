using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutorialLevel()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayMainLevel()
    {
        SceneManager.LoadScene("Main");
    }

    public void GoSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}

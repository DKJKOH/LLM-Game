using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class checkWinLose : MonoBehaviour
{
    private int enemiesLeft;

    public TextMeshProUGUI enemiesLeftText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Set enemies left text
        countEnemiesLeft();

        // Check if all enemies are eliminated
        if (AllEnemiesEliminated())
        {
            // ALL ENEMIES DEAD!
            SceneManager.LoadScene("Victory");
        }

        if (GameObject.FindGameObjectsWithTag("Player") == null)
        {
            SceneManager.LoadScene("Lose");
        }
    }

    bool AllEnemiesEliminated()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            AnimatorStateInfo stateInfo = enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("idle"))
            {
                return false;
            }
        }
        return true;
    }

    void countEnemiesLeft()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int enemiesAliveCount = 0;

        foreach (GameObject enemy in enemies)
        {
            AnimatorStateInfo stateInfo = enemy.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("idle"))
            {
                enemiesAliveCount++;
            }
        }

        // Set enemies left text
        enemiesLeftText.text = "Enemies Left: " + enemiesAliveCount;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    public GameObject popUpBox;

    // Use animations to handle behaviour
    public Animator animator;

    public TMP_Text popUpText;


    // Function which will pop up the box with text
    public void PopUp(string text)
    {
        // Activate popup box
        popUpBox.SetActive(true);

        // Change text
        popUpText.text = text;

        animator.SetTrigger("popup");
    }

}

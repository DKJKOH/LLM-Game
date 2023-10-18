using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

// 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

public class UI_Text_Input : MonoBehaviour
{
    // This is the text output
    public TextMeshProUGUI output;

    // Retrieves the user text input
    public TMP_InputField userTextInput;

    public void ButtonSubmitUI()
    {
        // Set output text as the user text input
        output.text = userTextInput.text;
    }
}

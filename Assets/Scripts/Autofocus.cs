using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Autofocus : MonoBehaviour
{
    public TMP_InputField inputField;

    // This method is called when the GameObject becomes enabled and active
    void OnEnable()
    {
        // Check if the input field is assigned
        if (inputField != null)
        {
            // Select the input field
            inputField.Select();
            // Activate the input field
            inputField.ActivateInputField();
        }
        else
        {
            Debug.LogError("InputField is not assigned in the inspector.");
        }
    }
}

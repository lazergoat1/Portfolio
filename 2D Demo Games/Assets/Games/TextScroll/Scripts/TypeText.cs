using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (textMesh.text.Length != 0)
                {
                    textMesh.text = textMesh.text.Substring(0, textMesh.text.Length - 1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                print("User entered their name: " + textMesh.text);
            }
            else
            {
                textMesh.text += c;
            }
        }
    }
}

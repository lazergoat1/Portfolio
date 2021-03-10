using System.Collections;
using UnityEngine;
using TMPro;

public class TalkingText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public TextSO[] text;
    public float textTimer;
    int num = 0;

    public void StartNextText()
    {
        StopAllCoroutines();

        if(num < text.Length)
        {
            StartCoroutine(PrintCharacter(text[num].text, textTimer));
        }
        else
        {
            num = 0;
            StartCoroutine(PrintCharacter(text[num].text, textTimer));
        }
        num += 1;
    }

    private IEnumerator PrintCharacter(string text, float timer)
    {
        char[] textArray = text.ToCharArray();

        string updateText = "";

        for (int i = 0; i < text.Length; i++)
        {
            updateText += textArray[i].ToString();

            textMesh.text = updateText;
            yield return new WaitForSeconds(timer);
        }
    }
}

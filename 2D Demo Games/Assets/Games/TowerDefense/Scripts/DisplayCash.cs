using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayCash : MonoBehaviour
{
    public GameController gameController;
    public TextMeshProUGUI text;

    private void Update()
    {
        UpdateCost();
    }
    void UpdateCost()
    {
        text.text = gameController.cash.ToString("0");
    }
}

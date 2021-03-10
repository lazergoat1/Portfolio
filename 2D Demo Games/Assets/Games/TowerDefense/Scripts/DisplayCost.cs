using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayCost : MonoBehaviour
{
    public PlaceTower placeTower;
    public TextMeshProUGUI text;

    private void Update()
    {
        UpdateCost();
    }
    void UpdateCost()
    {
        text.text = placeTower.cost.ToString("0");
    }
}

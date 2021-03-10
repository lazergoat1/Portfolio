using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HungerDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;
    public PlayerStats playerStats;

    private void Update()
    {
        text.text = "Hunger " + playerStats.hunger.ToString("0");
    }
}

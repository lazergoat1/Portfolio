using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;
    public PlayerStats playerStats;

    private void Update()
    {
        text.text = "Health " + playerStats.health.ToString("0");
    }
}

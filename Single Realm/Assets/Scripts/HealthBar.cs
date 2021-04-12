using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;

    private void Awake()
    {
        bar = transform.Find("Bar");
    }

    public void ChangeSize(float startingHealth, float currentHealth)
    {
        float sizeNormalized = currentHealth / startingHealth; 
        bar.localScale = new Vector3(sizeNormalized, 1);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    public static GameAssets instance
    {
        get
        {
            if(_instance == null) _instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _instance;
        }
    }

    public GameObject player;

    [Header("PopupUi's")]
    public GameObject xpPopup;
    public GameObject damagePopup;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
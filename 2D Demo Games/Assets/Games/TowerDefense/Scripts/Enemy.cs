using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    public string name;
    public float speed;
    public float health;
    public float dmg;
    public float payout;
}

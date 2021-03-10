using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public float health;
    public float xp;
    public float level;

    //[System.NonSerialized]
    public float currentHealth;

    private void OnEnable()
    {
        this.currentHealth = health;
        this.xp = 0f;
        this.level = 0f;
    }
}

using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class PlayerStats : ScriptableObject
{
    public GameObject player;
    public float health;
    public float xp;
    public int level;
    public Vector3 position;

    [System.NonSerialized]
    public float currentHealth;

    private void OnEnable()
    {
        this.currentHealth = health;
        this.xp = 0f;
        this.level = 0;
    }

}

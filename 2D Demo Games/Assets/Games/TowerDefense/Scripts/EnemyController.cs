using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    public List<GameObject> waypoints;

    public GameObject selectWaypoint;

    string name;
    public float speed;
    float health;
    float dmg;
    float payout;

    float recalculateDistance = 0.01f;

    public void Awake()
    {
        this.name = enemy.name;
        this.speed = enemy.speed;
        this.health = enemy.health;
        this.dmg = enemy.dmg;
        this.payout = enemy.payout;
    }

    void Start()
    {
        foreach (GameObject waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            waypoints.Add(waypoint);
        }
        
    }

    void Update()
    {
        FollowPath();
    }

    void FollowPath()
    {
        CalculatePath();
        if (selectWaypoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, selectWaypoint.transform.position, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, selectWaypoint.transform.position) <= recalculateDistance)
            {
                waypoints.Remove(selectWaypoint);
                selectWaypoint = null;
                CalculatePath();
            }
        }
    }

    void CalculatePath()
    {
        float distance = Mathf.Infinity;

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (Vector3.Distance(transform.position, waypoints[i].transform.position) < distance)
            {
                selectWaypoint = waypoints[i];
                distance = (transform.position - waypoints[i].transform.position).magnitude;
            }
        }
    }

    public void HandleDmg(float dmg)
    {
        health -= dmg;
        if(health <= 0f)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().cash += payout;
            Destroy(gameObject);
        }
    }
}

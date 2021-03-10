using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Tower tower;
    public GameObject projectilePrefab;

    GameObject closestEnemy;

    float fireSpeed;
    float range;

    float timeDelay;

    private void Awake()
    {

        fireSpeed = tower.fireSpeed;
        range = tower.range;

        timeDelay = fireSpeed;

    }

    private void Update()
    {
        Shoot();

    }

    void FindClosestEnemy()
    {
        float distance = Mathf.Infinity;

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range);

        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            if (enemiesInRange[i].tag == "Enemy")
            {
                if (Vector3.Distance(transform.position, enemiesInRange[i].transform.position) < distance)
                {
                    if (closestEnemy == null)
                    {
                        closestEnemy = enemiesInRange[i].gameObject;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, enemiesInRange[i].transform.position) < Vector3.Distance(transform.position, closestEnemy.transform.position))
                        {
                            closestEnemy = enemiesInRange[i].gameObject;
                        }
                    }
                }      
            }
        }
    }

    void Shoot()
    {
        timeDelay -= Time.deltaTime;
        if (timeDelay <= 0)
        {
            FindClosestEnemy();
            if (closestEnemy)
            {
                Projectile.Create(projectilePrefab.transform, transform.position, closestEnemy.transform.position, closestEnemy.transform);
                timeDelay = fireSpeed;
            }
        }         
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float dmg;
    public float dmgDistance;

    Vector3 targetPosition;

    Transform target;

    public static void Create(Transform projectile, Vector3 spawnPosition, Vector3 targetPosition, Transform target)
    {
        Transform projectileTransform = Instantiate(projectile, spawnPosition, Quaternion.identity);

        Projectile projectileClass = projectileTransform.GetComponent<Projectile>();
        projectileClass.Setup(targetPosition, target);

    }
    
    void Setup(Vector3 targetPosition, Transform target)
    {
        this.targetPosition = targetPosition;
        this.target = target;
    }


    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.position += moveDirection * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) <= dmgDistance)
        {
            print("within range");
            if (target)
            {
                target.GetComponent<EnemyController>().HandleDmg(dmg);
            }
            Destroy(gameObject);
        }
    }
}

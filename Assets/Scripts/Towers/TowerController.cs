using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerController : MonoBehaviour
{
    public string towerType;

    public float towerRadius;
    public float shootingDelay;
    public int towerDamage;

    public LayerMask enemyLayer;

    public GameObject FindNearestEnemy(float radius)
    {
        Collider[] Enemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        float closest = Mathf.Abs(radius * radius * radius);
        GameObject closestEnemy = null;
        if (Enemies.Length == 0)
        {
            return closestEnemy;
        }

        foreach (var enemy in Enemies)
        {
            EnemyController en = enemy.GetComponent<EnemyController>();
            if (!en.isDead)
            {
                float length = Vector3.Distance(enemy.transform.position, transform.position);
                if (length <= closest)
                {
                    closestEnemy = enemy.gameObject;
                    closest = length;
                }
            }
        }
        return closestEnemy;
    }
    public abstract IEnumerator ShootAtEnemy();
}

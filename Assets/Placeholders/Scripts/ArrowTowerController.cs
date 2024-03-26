using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTowerController : MonoBehaviour
{
    public float towerRadius = 5f;
    public float shootingDelay = 2f;
    public int bulletDamage = 2;
    [SerializeField]
    LayerMask enemyLayer;
    [SerializeField]
    BulletController bullet;

    GameObject nearestEnemy;

    public void Start()
    {
        StartCoroutine(ShootAtEnemy());
    }

    public GameObject FindNearestEnemy(float radius)
    {
        //should use OverlapSphereNoAlloc
        Collider[] Enemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        float closest = Mathf.Abs(radius * radius * radius);
        GameObject closestEnemy = null;
        if (Enemies.Length == 0)
        {
            return closestEnemy;
        }

        foreach(var enemy in Enemies)
        {
            EnemyController en = enemy.GetComponent<EnemyController>();
            if (!en.isDead)
            {
                float length = GameUtilities.VectorDifference(enemy.transform.position, transform.position);
                if (length <= closest)
                {
                    closestEnemy = enemy.gameObject;
                    closest = length;
                }
            }
        }
        return closestEnemy;
    }

    IEnumerator ShootAtEnemy()
    {
        while (true)
        {
            nearestEnemy = FindNearestEnemy(towerRadius);
            if (nearestEnemy != null)
            {
                bullet.target = nearestEnemy;
                bullet.bulletDamage = bulletDamage;
                Instantiate(bullet, transform.position, Quaternion.LookRotation(nearestEnemy.transform.position));
                yield return new WaitForSeconds(shootingDelay);
            }
            yield return null;
        }
    }
}

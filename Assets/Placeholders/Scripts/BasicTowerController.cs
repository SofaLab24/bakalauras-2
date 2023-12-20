using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTowerController : MonoBehaviour
{
    public float towerRadius = 3f;
    public float shootingSpeed = 3f;
    public int bulletDamage;
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
            float length = GameUtilities.VectorDifference(enemy.transform.position, transform.position);
            if (length <= closest)
            {
                closestEnemy = enemy.gameObject;
                closest = length;
                Debug.Log("Closest: " + closestEnemy.name);
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
                yield return new WaitForSeconds(shootingSpeed);
            }
            yield return null;
        }
    }
}

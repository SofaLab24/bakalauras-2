using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTowerController : MonoBehaviour
{
    public float towerRadius = 3f;
    [SerializeField]
    LayerMask enemyLayer;

    [SerializeField]
    GameObject nearestEnemy;

    public void Update()
    {
        nearestEnemy = FindNearestEnemy(towerRadius);
    }

    public GameObject FindNearestEnemy(float radius)
    {
        //should use OverlapSphereNoAlloc
        Collider[] Enemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        float closest = Mathf.Abs(radius * radius * radius);
        GameObject closestEnemy = null;
        Debug.Log("Length " + Enemies.Length);
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
}

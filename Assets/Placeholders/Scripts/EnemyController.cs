using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<Transform> walkPoints;
    public float walkSpeed;
    public int health = 3;

    private Rigidbody rb;
    private Transform target;
    private int currentWalkPoint;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWalkPoint = walkPoints.Count-1;
        target = walkPoints[currentWalkPoint];
        transform.position = target.position;
        transform.LookAt(target.position);
    }
    private void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position = transform.position + direction * walkSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.position) <= 0.05)
        {
            currentWalkPoint--;
            target = walkPoints[currentWalkPoint];
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            Destroy(gameObject);   
        }
    }
}

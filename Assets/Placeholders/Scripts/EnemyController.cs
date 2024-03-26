using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyController : MonoBehaviour
{
    public List<Transform> walkPoints;
    public float walkSpeed;
    public int health = 3;
    public int moneyWorth = 5;
    public int damage = 1;

    private VisualEffect deathExplosion;
    private bool hasDeathExplosionPlayed;
    public bool isDead;

    private Rigidbody rb;
    private Transform target;
    private int currentWalkPoint;
    private void Start()
    {
        hasDeathExplosionPlayed = false;
        isDead = false;
        rb = GetComponent<Rigidbody>();
        currentWalkPoint = walkPoints.Count-1;
        target = walkPoints[currentWalkPoint];
        transform.position = target.position;
        transform.LookAt(target.position);
        deathExplosion = GetComponent<VisualEffect>();
    }
    private void Update()
    {
        if (!isDead)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position = transform.position + direction * walkSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, target.position) <= 0.05)
            {
                currentWalkPoint--;
                target = walkPoints[currentWalkPoint];
            }
        }
        else if (isDead && deathExplosion.aliveParticleCount <= 0 && hasDeathExplosionPlayed)
        {
            Destroy(gameObject);
        }

        if (deathExplosion.aliveParticleCount > 0)
        {
            hasDeathExplosionPlayed = true;
        }
    }
    public void PlayDeathExplosion()
    {
        deathExplosion.Play();
        isDead = true;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            FindFirstObjectByType<MoneyManager>().AddMoney(moneyWorth);
            PlayDeathExplosion();
        }
    }
}

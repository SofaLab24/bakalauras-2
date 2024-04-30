using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyController : MonoBehaviour
{
    public string enemyName;

    public List<Transform> walkPoints;
    public float walkSpeed;
    public int health = 3;
    public int moneyWorth = 5;
    public int waveWorth;
    public int damage = 1;

    private VisualEffect deathExplosion;
    private bool hasDeathExplosionPlayed;
    public bool isDead;

    private Rigidbody rb;
    private Transform target;
    private int currentWalkPoint;

    public float heightOffset = 0f;

    public AudioClip explosionSound;
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
            transform.LookAt(new Vector3(walkPoints[currentWalkPoint].position.x, heightOffset, walkPoints[currentWalkPoint].position.z));
            Vector3 direction = (new Vector3(target.position.x, heightOffset, target.position.z) - transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);
            transform.position = transform.position + direction * walkSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, heightOffset, transform.position.z);
            
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.position.x, target.position.z)) <= 0.05)
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
        PlayerPrefs.SetInt(enemyName, PlayerPrefs.GetInt(enemyName) + 1);
        AudioManager.Instance.PlaySound(explosionSound, transform.position);
        deathExplosion.Play();
        GetComponent<MeshRenderer>().enabled = false;
        isDead = true;
    }
    public void PlayDeathExplosion(bool isDead)
    {
        if (!isDead)
        {
            AudioManager.Instance.PlaySound(explosionSound, transform.position);
            deathExplosion.Play();
            GetComponent<MeshRenderer>().enabled = false;
            isDead = true;
        }
        else PlayDeathExplosion();
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

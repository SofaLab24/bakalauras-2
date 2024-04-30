using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    OverlayController overlayController;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        overlayController = FindFirstObjectByType<OverlayController>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
        TakeDamage(enemyController.damage);
        overlayController.TakeDamage(enemyController.damage);
        enemyController.PlayDeathExplosion(false);
    }
}

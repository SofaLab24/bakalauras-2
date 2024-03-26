using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
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
        // update overlay
        if (currentHealth <= 0) 
        {
            Debug.Log("YOU DIED!");
            // TODO: trigger game loss logic
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
        TakeDamage(enemyController.damage);
        overlayController.TakeDamage(enemyController.damage);
        enemyController.PlayDeathExplosion();
    }
}

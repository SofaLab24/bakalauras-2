using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WizardTowerController : MonoBehaviour
{
    public float towerRadius = 25f;
    public float shootingDelay = 5f;
    public int towerDamage = 7;
    [SerializeField]
    LayerMask enemyLayer;
    [SerializeField]
    VisualEffect lightningBeam;

    GameObject nearestEnemy;

    public AudioClip lightningSound;

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

    IEnumerator ShootAtEnemy()
    {
        while (true)
        {
            nearestEnemy = FindNearestEnemy(towerRadius);
            if (nearestEnemy != null)
            {
                nearestEnemy.GetComponent<EnemyController>().TakeDamage(towerDamage);
                lightningBeam.transform.LookAt(nearestEnemy.transform.position);
                lightningBeam.SetFloat("Distance", Vector3.Distance(lightningBeam.transform.position, nearestEnemy.transform.position));
                lightningBeam.Play();
                AudioManager.Instance.PlaySound(lightningSound, transform.position);
                yield return new WaitForSeconds(shootingDelay);
            }
            yield return null;
        }
    }
}

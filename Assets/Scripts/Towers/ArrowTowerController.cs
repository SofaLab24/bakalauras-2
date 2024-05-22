using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTowerController : TowerController
{
    [SerializeField]
    ArrowController bullet;

    public void Start()
    {
        StartCoroutine(ShootAtEnemy());
    }

    public override IEnumerator ShootAtEnemy()
    {
        while (true)
        {
            GameObject nearestEnemy = FindNearestEnemy(base.towerRadius);
            if (nearestEnemy != null)
            {
                bullet.target = nearestEnemy;
                bullet.bulletDamage = base.towerDamage;
                Instantiate(bullet, transform.position, Quaternion.LookRotation(nearestEnemy.transform.position));
                yield return new WaitForSeconds(shootingDelay);
            }
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FlameTowerController: TowerController
{
    [SerializeField]
    VisualEffect flameSpray;
    [SerializeField]
    AudioClip spraySound;

    public void Start()
    {
        StartCoroutine(ShootAtEnemy());
    }

    public override IEnumerator ShootAtEnemy()
    {
        while (true)
        {
            GameObject nearestEnemy = FindNearestEnemy(towerRadius);
            if (nearestEnemy != null)
            {
                nearestEnemy.GetComponent<EnemyController>().TakeDamage(towerDamage);
                flameSpray.transform.LookAt(nearestEnemy.transform.position);
                flameSpray.Play();
                AudioManager.Instance.PlaySound(spraySound, transform.position);
                yield return new WaitForSeconds(shootingDelay);
            }
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WizardTowerController : TowerController
{
    [SerializeField]
    VisualEffect lightningBeam;
    [SerializeField]
    AudioClip lightningSound;

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

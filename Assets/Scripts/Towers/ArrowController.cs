using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ArrowController : MonoBehaviour
{
    public GameObject target;
    public float bulletSpeed = 5f;
    public int bulletDamage = 1;

    private Rigidbody rb;
    private bool isDead;
    private VisualEffect trail;

    private void Start()
    {
        isDead = false;
        trail = GetComponent<VisualEffect>();
        rb = GetComponent<Rigidbody>();
        transform.LookAt(target.transform.position);
    }
    private void Update()
    {
        if (!isDead && !target.IsDestroyed())
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * bulletSpeed * Time.deltaTime);
        }

        if (isDead && trail.aliveParticleCount <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target && !isDead)
        {
            isDead = true;
            other.GetComponent<EnemyController>().TakeDamage(bulletDamage);
            GetComponent<MeshRenderer>().enabled = false;
            trail.Stop();
        }
    }
}

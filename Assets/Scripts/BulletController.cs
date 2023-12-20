using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject target;
    public float bulletSpeed = 5f;
    public int bulletDamage = 1;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.LookAt(target.transform.position);
    }
    private void Update()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Huh");
        if (other.gameObject == target)
        {
            other.GetComponent<EnemyController>().TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}

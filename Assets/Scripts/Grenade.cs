using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float explosionForce = 500f;
    public float maxDamage = 8f; 

    public void DelayedExplosion(float delay)
    {
        Invoke("Explode", delay);
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            PatrolAgent agent = nearbyObject.GetComponent<PatrolAgent>();
            if (agent != null)
            {
                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                float damage = Mathf.Lerp(maxDamage, 0, distance / explosionRadius);
                agent.HitByGrenade(damage);
            }
        }
        Destroy(gameObject);  // Destroy the grenade after explosion
    }
}

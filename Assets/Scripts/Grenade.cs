using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;

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
        }
        Destroy(gameObject);  // Destroy the grenade after explosion
    }
}

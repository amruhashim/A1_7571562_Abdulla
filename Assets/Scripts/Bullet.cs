using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PatrolAgent"))
        {
            //CreateBulletImpactEffect(collision, GlobalReferences.Instance.bulletImpacteffectWall);
            print("hit " + collision.gameObject.name + " !");
            // Reduce health if the target is an AI agent
            PatrolAgent agent = collision.gameObject.GetComponent<PatrolAgent>();
            if (agent != null)
            {
                agent.HitByProjectile();
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Bottle"))
        {
            print("hit " + collision.gameObject.name + " !");
            collision.gameObject.GetComponent<Bottle>().Shatter();
            Destroy(gameObject);  // Ensure bullet is destroyed after collision
        }

        if (collision.gameObject.CompareTag("Metal"))
        {
            CreateBulletImpactEffect(collision, GlobalReferences.Instance.bulletImpacteffectMetal);
            print("hit " + collision.gameObject.name + " !");
            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision collision, GameObject impactEffectPrefab)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(
            impactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        hole.transform.SetParent(collision.gameObject.transform);
    }
}

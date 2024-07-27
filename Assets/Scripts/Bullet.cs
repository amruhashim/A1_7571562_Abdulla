using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Target"))
        {
            CreateBulletImpactEffectStone(collision);
            print("hit " + collision.gameObject.name + " !");
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("Bottle"))
        {
            print("hit " + collision.gameObject.name + " !");
            collision.gameObject.GetComponent<Bottle>().Shatter();
        }

        if(collision.gameObject.CompareTag("Metal"))
        {
            CreateBulletImpactEffectMetal(collision);
            print("hit " + collision.gameObject.name + " !");
            Destroy(gameObject);
        }
    }

    
    void CreateBulletImpactEffectStone(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpacteffectWall, 
            contact.point,
            Quaternion.LookRotation(contact.normal)
             );

        hole.transform.SetParent(collision.gameObject.transform);
        

    }


    void CreateBulletImpactEffectMetal(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpacteffectMetal, 
            contact.point,
            Quaternion.LookRotation(contact.normal)
             );

        hole.transform.SetParent(collision.gameObject.transform);
        

    }
}

using UnityEngine;

public class Ammo : MonoBehaviour
{
    public Weapon.gunType GunType;
    public int ammoAmount = 10; // Amount of ammo to be collected

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the Weapon script on the child of the player
            Weapon weapon = other.GetComponentInChildren<Weapon>();
            if (weapon != null && weapon.GunType == GunType)
            {
                weapon.CollectAmmo(ammoAmount);
                Destroy(gameObject); // Destroy ammo object after collection
            }
        }
    }
}

using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public Transform handHolder; // The transform that will hold the weapon
    public Weapon currentWeapon; // The current weapon being held
    public Camera playerCamera; // Reference to the player's camera
    public Movement playerMovement; // Reference to the player's movement script

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a weapon box
        if (other.gameObject.CompareTag("WeaponBox"))
        {
            // Get the weapon box script
            WeaponBox weaponBox = other.gameObject.GetComponent<WeaponBox>();

            // Get the weapon prefab from the weapon box
            GameObject weaponPrefab = weaponBox.weaponPrefab;

            // Switch to the new weapon
            SwitchWeapon(weaponPrefab);
        }
    }

    private void SwitchWeapon(GameObject weaponPrefab)
    {
        // Destroy the current weapon
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        // Instantiate the new weapon
        GameObject newWeapon = Instantiate(weaponPrefab, handHolder);

        // Set the new weapon's local position and rotation to match the hand holder
        newWeapon.transform.localPosition = new Vector3(0, -1.45899999f, -0.479999989f);
        newWeapon.transform.localRotation = Quaternion.identity;

        // Set the new weapon as the current weapon
        currentWeapon = newWeapon.GetComponent<Weapon>();

        // Assign the player camera to the new weapon
        currentWeapon.playerCamera = playerCamera;

        // Assign the player movement script to the new weapon's AnimationController
        AnimationController animationController = newWeapon.GetComponent<AnimationController>();
        if (animationController != null)
        {
            animationController.movementScript = playerMovement;
        }

        // Update the ammo display
        AmmoManager.Instance.UpdateAmmoDisplay(currentWeapon);
    }
}

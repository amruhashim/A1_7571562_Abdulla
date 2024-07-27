using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public GameObject muzzleEffect;
    public Transform bulletSpawn;

    [Header("Weapon Type")]
    public gunType GunType;

    [Header("Reloading & Magazine")]
    public float reloadTime;
    public int magazineSize;
    public int bulletsLeft;
    public bool isReloading;

    [Header("Bullet Settings")]
    public float bulletVelocity = 30f;
    public float bulletLifeTime = 3f;

    [Header("Shooting Settings")]
    public bool isShooting;
    public bool readyToShoot;
    public float shootingDelay = 0.5f;

    [HideInInspector]
    public float initialDelay = 0.5f;

    [HideInInspector]
    [Header("Burst Settings")]
    public int bulletsPerShot = 1;

    [Header("Accuracy Settings")]
    public float spreadIntensity;

    [Header("Ammo Management")]
    public int accumulatedBullets = 0; // Total bullets collected

    [Header("Audio Settings")]
    public AudioClip shootingSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;
    private AudioSource audioSource;

    public enum gunType
    {
        HandGun,
        ShotGun,
        MachineGun,
    }

    private bool allowReset = true;
    private AnimationController animatorController;

    private void Awake()
    {
        readyToShoot = true;
        bulletsLeft = magazineSize;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameObject instantiated = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        instantiated.transform.SetPositionAndRotation(bulletSpawn.position, Quaternion.LookRotation(bulletSpawn.forward));
        animatorController = GetComponent<AnimationController>();
    }

    void Update()
    {
        if (GunType == gunType.MachineGun)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (GunType == gunType.HandGun || GunType == gunType.ShotGun)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && accumulatedBullets > 0)
        {
            Reload();
        }

        if (readyToShoot && isShooting)
        {
            if (bulletsLeft > 0)
            {
                FireWeapon();
            }
            else
            {
                PlayEmptySound();
            }
        }

        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft}/{accumulatedBullets}";
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;

        if (audioSource != null && shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }

        if (GunType == gunType.ShotGun)
        {
            animatorController.SetShooting(true);
            for (int i = 0; i < bulletsPerShot; i++)
            {
                FireBullet();
            }
            bulletsLeft -= bulletsPerShot;
            Invoke("ResetShot", shootingDelay);
        }
        else if (GunType == gunType.MachineGun)
        {
            StartCoroutine(FireMachineGun());
        }
        else // HandGun
        {
            animatorController.SetShooting(true);
            FireBullet();
            bulletsLeft--;
            Invoke("ResetShot", shootingDelay);
        }
    }

    private IEnumerator FireMachineGun()
    {
        yield return new WaitForSeconds(initialDelay);

        while (isShooting && bulletsLeft > 0)
        {
            if (audioSource != null && shootingSound != null)
            {
                audioSource.PlayOneShot(shootingSound);
            }

            animatorController.SetShooting(true);
            FireBullet();
            bulletsLeft--;
            yield return new WaitForSeconds(shootingDelay);
        }

        animatorController.SetShooting(false);
        readyToShoot = true;
    }

    private void Reload()
    {
        animatorController.SetReloading(true);
        isReloading = true;
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        animatorController.SetReloading(false);
        int bulletsToReload = Mathf.Min(magazineSize - bulletsLeft, accumulatedBullets);
        bulletsLeft += bulletsToReload;
        accumulatedBullets -= bulletsToReload;
        isReloading = false;

        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft}/{accumulatedBullets}";
        }
    }

    private void PlayEmptySound()
    {
        if (audioSource != null && emptySound != null)
        {
            audioSource.PlayOneShot(emptySound);
        }
    }

    public void CollectAmmo(int ammoAmount)
    {
        accumulatedBullets += ammoAmount;
        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft}/{accumulatedBullets}";
        }
    }

    private void FireBullet()
    {
        muzzleEffect.GetComponent<ParticleSystem>().Play();

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.LookRotation(shootingDirection));
        bullet.GetComponent<Rigidbody>().velocity = shootingDirection * bulletVelocity;

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
        animatorController.SetShooting(false);
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = (targetPoint - bulletSpawn.position).normalized;

        float x = Random.Range(-spreadIntensity, spreadIntensity);
        float y = Random.Range(-spreadIntensity, spreadIntensity);

        Vector3 spread = new Vector3(x, y, 0);
        Vector3 finalDirection = direction + playerCamera.transform.TransformDirection(spread);

        return finalDirection;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}

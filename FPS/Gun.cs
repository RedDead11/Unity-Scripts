using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 0.1f;

    [Header("Ammo Settings")]
    public int maxAmmo = 60;
    public int currentAmmo;
    public TextMeshProUGUI ammoText;

    public bool useRandomness = false; // Flag to enable randomness in bullet direction
    public float maxSpreadAngle = 5f; // Maximum angle for bullet spread (in degrees)

    private AudioSource fireSound;
    private bool isFiring;
    [Tooltip("Set -1 for Scorpion and 1 for Ak")]public int invertBulletDirection;

    private void Start()
    {
        currentAmmo = 30;
        fireSound = GetComponent<AudioSource>();
        UpdateAmmoText();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            isFiring = true;
            fireSound.Play();
            StartCoroutine(FireRoutine());
        }

        if (Input.GetMouseButtonUp(0))
        {
            fireSound.Stop();
            isFiring = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private IEnumerator FireRoutine()
    {
        while (isFiring && currentAmmo > 0)
        {
            // Calculate bullet direction with optional randomness
            Vector3 fireDirection = bulletSpawnPoint.forward;
            if (useRandomness)
            {
                fireDirection = ApplyRandomSpread(bulletSpawnPoint.forward, maxSpreadAngle);
            }

            // Instantiate the bullet
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = (invertBulletDirection * fireDirection) * bulletSpeed;

            currentAmmo--;
            UpdateAmmoText();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private Vector3 ApplyRandomSpread(Vector3 baseDirection, float spreadAngle)
    {
        // Calculate random spread within the specified angle
        Vector3 randomSpread = Random.insideUnitSphere * spreadAngle;
        Quaternion spreadRotation = Quaternion.Euler(randomSpread);
        return spreadRotation * baseDirection;
    }

    private void Reload()
    {
        if (maxAmmo > 0 && currentAmmo < 30) // Only reload if there's ammo to reload and not already full
        {
            int neededAmmo = 30 - currentAmmo;
            int ammoToReload = Mathf.Min(maxAmmo, neededAmmo); // Reload up to needed ammo or available max ammo

            currentAmmo += ammoToReload;
            maxAmmo -= ammoToReload;

            UpdateAmmoText();
        }
    }

    private void UpdateAmmoText()
    {
        currentAmmo = Mathf.Clamp(currentAmmo, 0, 30); // Ensure current ammo doesn't go below 0 or above 30
        maxAmmo = Mathf.Clamp(maxAmmo, 0, int.MaxValue); // Ensure max ammo doesn't go below 0

        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
}

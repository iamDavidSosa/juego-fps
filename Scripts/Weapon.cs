using TMPro;
using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 30f;
    public float bulletPrefabLifeTime = 2f;

    public AudioSource shootingSound;
    public int maxAmmo = 10; // Máxima cantidad de balas
    private int currentAmmo;

    public float reloadTime = 2f;
    private bool isReloading = false;

    // Referencias para la UI
    public TextMeshProUGUI MunicionText; // Texto principal para la cantidad de munición
    public TextMeshProUGUI SinBalasText; // Mensaje que indica que no tienes balas

    void Start()
    {
        currentAmmo = maxAmmo; // Inicializamos las balas
        UpdateUI();
        SinBalasText.gameObject.SetActive(false); // Ocultar mensaje de "sin balas" al inicio
    }

    void Update()
    {
        if (isReloading) return; // No permitir disparos mientras se recarga

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentAmmo > 0)
            {
                Fire();
                SinBalasText.gameObject.SetActive(false); // Ocultar mensaje de "Sin balas" si disparas
            }
            else
            {
                SinBalasText.gameObject.SetActive(true); // Mostrar mensaje de "Sin balas"
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private void Fire()
    {
        shootingSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(bulletSpawn.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        currentAmmo--; // Reducir la cantidad de munición
        UpdateUI();
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        MunicionText.text = "Munición: Recargando..."; // Cambiar el texto a "Recargando..."
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo; // Recargar las balas
        isReloading = false;
        UpdateUI();
        SinBalasText.gameObject.SetActive(false); // Ocultar mensaje de "Sin balas" después de recargar
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    private void UpdateUI()
    {
        MunicionText.text = $"Munición: {currentAmmo}/{maxAmmo}";
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class silahdeneme : MonoBehaviour
{
    public GameObject bullet;

    public Camera cam;
    public float nextFire;
    public float fireRate = 10;

    [Range(0f, 2f)]
    public float recoverPercent = 0.7f;

    [Space]
    public float recoilUp = 0.025f;
    public float recoilBack = 0.025f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;

    private bool recoiling;
    private bool recovering;

    public int mag = 5;
    public int ammo = 30;

    private AudioSource audioSource;

    public Transform spawnPoint;
    public float shootForce = 10f;

    public Image crossHair;
    RaycastHit hit;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }
    private void Update()
    {
        if (nextFire > 0)
            nextFire -= Time.deltaTime;


        if (Input.GetKey(KeyCode.Mouse0) && nextFire <= 0 && ammo > 0)
        {
            nextFire = 1 / fireRate;
            shoot();
        }
        if ((Input.GetKeyDown(KeyCode.R) && mag > 0) || (mag > 0 && ammo == 0))
        {
            Reload();
        }


        if (recoiling)
        {
            Recoil();
        }
        if (recovering)
        {
            Recover();
        }

    }
    public void OnEnemyHit()
    {
        crossHair.color = Color.red; // Crosshair rengini kýrmýzý yap
        StartCoroutine(ResetCrossHairColor()); // Bir süre sonra rengi beyaza döndür
    }

    // Niþangah rengini tekrar beyaza döndüren Coroutine
    private IEnumerator ResetCrossHairColor()
    {
        yield return new WaitForSeconds(.2f); // 1 saniye bekle
        crossHair.color = Color.white; // Niþangah rengini tekrar beyaz yap
    }
    void Reload()
    {

        if (mag > 0)
        {
            mag--;
            ammo = 30;
        }
    }


    void shoot()
    {
        crossHair.color = Color.white;
        audioSource.Play();
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 2000f))
        {

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(300);


            Debug.Log(hit.transform);
            if (hit.transform.GetComponent<Enemy>())
            {
                var enemy = hit.transform.GetComponent<Enemy>();
                enemy.TakeDamage(25);
                enemy.GetComponent<Rigidbody>().AddForce(-enemy.gameObject.transform.forward * 20f, ForceMode.Impulse);
                OnEnemyHit();

            }
            else
                crossHair.color = Color.white;
            Vector3 directionWithoutSpread = targetPoint - spawnPoint.position;
            GameObject currentBullet = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
            currentBullet.transform.forward = directionWithoutSpread.normalized;

            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread * shootForce, ForceMode.Impulse);
            Destroy(currentBullet, .4f);
        }
        ammo--;
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }
    void Recover()
    {
        Vector3 finalPosition = originalPosition;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
}

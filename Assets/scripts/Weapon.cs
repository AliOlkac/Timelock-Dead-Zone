using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 5;
    public float fireRate = 10;
    public Camera Camera;


    private float nextFire;

    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;




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

    public GameObject bulletPrefab;
    public float range = 900f;

    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        originalPosition = transform.localPosition;
        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }
    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0) && nextFire <= 0 && ammo > 0)
        {
            nextFire = 1 / fireRate;

            ammo--;


            Fire();
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


    void Reload()
    {

        if (mag > 0)
        {
            mag--;
            ammo = magAmmo;
        }
    }


    void Fire()
    {
        audioSource.Play();
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            GameObject laser = Instantiate(bulletPrefab, Camera.transform.forward, Quaternion.identity);
            laser.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);

            Destroy(laser, 5f);
        }

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

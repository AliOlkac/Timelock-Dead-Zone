using UnityEngine;

public class silahdeneme : MonoBehaviour
{

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
    private void Start()
    {

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }
    private void Update()
    {
        if (nextFire > 0)
            nextFire -= Time.deltaTime;


        if (Input.GetKey(KeyCode.Mouse0) && nextFire <= 0)
        {
            nextFire = 1 / fireRate;
            shoot();
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

    void shoot()
    {
        recoiling = true;
        recovering = false;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            Debug.Log(hit.transform);
            if (hit.transform.GetComponent<Enemy>())
            {
                var enemy = hit.transform.GetComponent<Enemy>();
                enemy.TakeDamage(25);
            }
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

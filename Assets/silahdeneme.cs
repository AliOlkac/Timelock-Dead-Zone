using UnityEngine;

public class silahdeneme : MonoBehaviour
{

    public Camera cam;
    public float nextFire;
    public float fireRate = 10;
    private void Update()
    {
        if (nextFire > 0)
            nextFire -= Time.deltaTime;


        if (Input.GetKey(KeyCode.Mouse0) && nextFire <= 0)
        {
            nextFire = 1 / fireRate;
            shoot();
        }
    }

    void shoot()
    {
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
}

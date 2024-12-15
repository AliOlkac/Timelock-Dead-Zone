using UnityEngine;

public class silahdeneme : MonoBehaviour
{

    public Camera cam;

    private void Update()
    {
        shoot();
    }

    void shoot()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            Debug.Log(hit.transform);
        }
    }
}

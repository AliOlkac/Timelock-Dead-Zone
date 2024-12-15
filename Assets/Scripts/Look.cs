using UnityEngine;

public class Look : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation;





    private void Start()
    {
    }



    private void Update()
    {
        LookHandle();
    }


    void LookHandle()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation= Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up*mouseX);
    }
}

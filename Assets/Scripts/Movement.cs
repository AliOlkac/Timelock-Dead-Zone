using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float speed = (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed);
        Move(speed);
    }
    void Move(float _speed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        controller.Move(moveDir.normalized * Time.deltaTime * _speed);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 1f;
    Rigidbody rb;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    AudioSource audioSource;
    private bool isPlayingSound = false; // Sesin çalýp çalmadýðýný kontrol etmek için
    public NewWeapon newWeapon;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        float speed = (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed);
        moveHandle(speed);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void moveHandle(float _speed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        if (moveDir.magnitude != 0)
        {
            if (!isPlayingSound) // Ses çalmýyorsa
            {
                StartCoroutine(playWalkSound());
            }
        }
        else
        {
            // Eðer duruyorsa sesi durdur
            if (isPlayingSound)
            {
                audioSource.Stop();
                isPlayingSound = false;
            }
        }

        controller.Move(moveDir.normalized * Time.deltaTime * _speed);
    }
    IEnumerator playWalkSound()
    {
        isPlayingSound = true; // Ses çalýyor
        if (Input.GetKey(KeyCode.LeftShift))
            audioSource.pitch = 2f;
        else
            audioSource.pitch = 1.5f;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length / audioSource.pitch); // Sesin uzunluðu kadar bekle
        isPlayingSound = false; // Ses bitti
    }
}

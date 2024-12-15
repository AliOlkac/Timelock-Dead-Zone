using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float health = 100;


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

    public bool enableHeadBob = false;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 jointOriginalPos;
    private float timer = 0;

    private bool isWalking = false;
    public silahdeneme silahDeneme;
    public Image healthCircle;
    public TextMeshProUGUI healthText;

    public bool isGamePaused = false;

    public GameObject pauseScreen ;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        jointOriginalPos = joint.localPosition;

    }
    private void Start()
    {
        healthText.text = health.ToString();
        SetHealth();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cube") && Input.GetKey(KeyCode.E))
        {
            Debug.Log(other.transform);
            Destroy(other.gameObject);
            silahDeneme.mag += 1;

        }
    }


    void PauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            pauseScreen.SetActive(false);
        }
    }
    void SetHealth()
    {
        healthCircle.fillAmount = (float)health / 100;
    }
    public void takeDamage(float damage)
    {
        health -= damage;
        healthText.text = health.ToString();
        SetHealth();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }


        if (enableHeadBob)
        {
            HeadBob();
        }
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

    private void HeadBob()
    {
        if (isWalking)
        {
            // Calculates HeadBob speed during sprint

            timer += Time.deltaTime * bobSpeed;
            // Applies HeadBob movement
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            // Resets when play stops moving
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }
    void moveHandle(float _speed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        if (moveDir.magnitude != 0)
        {
            isWalking = true;
            enableHeadBob = true;

            if (!isPlayingSound) // Ses çalmýyorsa
            {
                StartCoroutine(playWalkSound());
            }
        }
        else
        {
            isWalking = false;
            enableHeadBob = false;
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

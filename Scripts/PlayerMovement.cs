using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = Vector3.zero;

    public int maxHealth = 1;
    private int currentHealth;
    public bool isDead = false; // Indica si el jugador está muerto

    public float deathDelay = 3f;
    public TextMeshProUGUI MoristeText;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        MoristeText.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (isDead)
        {
            return; // Solo previene acciones como saltos, disparos, etc.
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        lastPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        print("Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        MoristeText.gameObject.SetActive(true);
    }
}

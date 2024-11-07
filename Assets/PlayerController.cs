using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;  // Velocidad de movimiento del jugador
    [SerializeField] private float sprintSpeed = 7f; // Velocidad al correr (sprint)
    [SerializeField] private float jumpHeight = 2f; // Altura del salto

    [Header("Disparo")]
    [SerializeField] private GameObject bulletPrefab;    // Referencia al prefab de la bala
    [SerializeField] private Transform rifleStart;       // Punto de inicio de la bala (normalmente en la punta del arma)

    [Header("Salud")]
    [SerializeField] private float health = 100f;        // Salud del jugador
    [SerializeField] private UnityEngine.UI.Text hpText;  // Texto para mostrar la salud en la UI

    private CharacterController characterController;  // Controlador del personaje (para movimiento)
    private float gravity = -9.81f;  // Gravedad
    private Vector3 velocity;  // Velocidad para el movimiento, incluyendo la gravedad
    private bool isGrounded;  // Verificar si el jugador está en el suelo

    void Start()
    {
        // Obtener el componente CharacterController
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();  // Maneja el movimiento
        HandleJump();      // Maneja el salto
        HandleShoot();     // Maneja el disparo
        UpdateHealthUI();   // Actualiza la UI de salud
    }

    private void HandleMovement()
    {
        // Verificar si el jugador está en el suelo
        isGrounded = characterController.isGrounded;

        float moveX = Input.GetAxis("Horizontal");  // Movimiento horizontal (A/D o flechas)
        float moveZ = Input.GetAxis("Vertical");    // Movimiento vertical (W/S o flechas)

        // Dirección de movimiento
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Velocidad de movimiento (considerar correr/sprint)
        float speed = (Input.GetKey(KeyCode.LeftShift)) ? sprintSpeed : moveSpeed; // Si Shift está presionado, corre

        // Aplicar el movimiento al CharacterController
        characterController.Move(move * speed * Time.deltaTime);

        // Aplicar gravedad al jugador
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Cuando está en el suelo, establecemos la gravedad para mantenerlo en el suelo
        }
        velocity.y += gravity * Time.deltaTime;  // Aplíquese la gravedad

        // Mover al jugador en el eje Y (gravedad)
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))  // "Jump" por defecto es la tecla espacio
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Salto usando la fórmula de física
        }
    }

    private void HandleShoot()
    {
        // Disparar al presionar el botón izquierdo del ratón (Mouse Button 0)
        if (Input.GetMouseButtonDown(0))
        {
            // Instanciar la bala en el punto de inicio del rifle
            GameObject bullet = Instantiate(bulletPrefab, rifleStart.position, rifleStart.rotation);
            
            // Obtener el componente Bullet y establecer la dirección
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.setDirection(transform.forward);  // La bala se mueve hacia adelante desde el jugador
            }
        }
    }

    // Método para cambiar la salud del jugador
    public void ChangeHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0f, 100f);  // Limitar la salud entre 0 y 100
        UpdateHealthUI();  // Actualizar la UI de salud
    }

    // Actualizar la UI de salud en pantalla
    private void UpdateHealthUI()
    {
        if (hpText != null)
        {
            hpText.text = "Health: " + health.ToString("0");  // Mostrar la salud en el UI
        }
    }
}

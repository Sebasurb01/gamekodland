using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;  // Velocidad de movimiento
    [SerializeField] private float jumpHeight = 2f; // Altura del salto
    [SerializeField] private float gravity = -9.8f; // Gravedad
    [SerializeField] private Transform cameraTransform; // Cámara del jugador

    private CharacterController characterController;  // Referencia al CharacterController
    private Vector3 moveDirection;  // Dirección de movimiento

    private float ySpeed = 0f; // Velocidad en el eje Y (para el salto)

    void Start()
    {
        // Obtener el componente CharacterController
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Obtener entrada de movimiento
        float horizontal = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        float vertical = Input.GetAxis("Vertical"); // W/S o flechas arriba/abajo

        // Calcular dirección de movimiento relativa a la cámara
        Vector3 move = cameraTransform.right * horizontal + cameraTransform.forward * vertical;
        move.y = 0f; // Asegurar que el jugador solo se mueve horizontalmente

        // Aplicar velocidad de movimiento
        moveDirection = move.normalized * moveSpeed;

        // Aplicar gravedad y salto
        if (characterController.isGrounded)
        {
            // Si el jugador está tocando el suelo, restablecer la velocidad en Y
            ySpeed = -0.5f; // Pequeña fuerza para evitar que se quede "pegado" al suelo

            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calcular velocidad del salto
            }
        }
        else
        {
            // Si el jugador no está en el suelo, aplicar gravedad
            ySpeed += gravity * Time.deltaTime;
        }

        // Aplicar movimiento en Y (gravedad y salto)
        moveDirection.y = ySpeed;

        // Mover el personaje
        characterController.Move(moveDirection * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float mouseSense = 1.0f;    // Sensibilidad del ratón
    [SerializeField] Transform player;            // Referencia al jugador
    [SerializeField] Transform playerArms;        // Referencia a las armas del jugador

    private float xAxisClamp = 0f;                // Control de rotación en el eje X (vertical)

    void Update()
    {
        // Bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obtener el movimiento del ratón
        float rotateX = Input.GetAxis("Mouse X") * mouseSense;
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense;

        // Rotación del jugador en el eje Y (horizontal)
        player.Rotate(Vector3.up * rotateX);

        // Limitar la rotación en el eje X (vertical) del jugador
        xAxisClamp -= rotateY;
        xAxisClamp = Mathf.Clamp(xAxisClamp, -90f, 90f); // Limitar entre -90 y 90 grados

        // Aplicar la rotación en el eje X a las armas del jugador
        Vector3 playerArmsRotation = playerArms.localRotation.eulerAngles;
        playerArmsRotation.x = xAxisClamp;  // Rotación en el eje X de las armas
        playerArmsRotation.z = 0f;          // No permitir rotación en el eje Z de las armas
        playerArms.localRotation = Quaternion.Euler(playerArmsRotation);
    }
}


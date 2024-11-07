using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float mouseSense = 1.0f;  // Sensibilidad del ratón
    [SerializeField] Transform player;         // Referencia al jugador (cuerpo)
    [SerializeField] Transform playerArms;     // Referencia a las armas del jugador (manos/cámara)

    private float xAxisClamp = 0f;             // Control de rotación en el eje X (vertical)

    // Método que controla la rotación de la cámara y el jugador
    void Update()
    {
        // Bloquear el cursor y ocultarlo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obtener la entrada del ratón para el movimiento
        float rotateX = Input.GetAxis("Mouse X") * mouseSense;
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense;

        // Llamar al método que rota el jugador en el eje Y (horizontal)
        RotatePlayer(rotateX);

        // Llamar al método que rota las armas del jugador en el eje X (vertical) y limita la rotación
        RotateArms(rotateY);
    }

    // Método que rota el jugador en el eje Y (horizontal)
    void RotatePlayer(float rotateX)
    {
        player.Rotate(Vector3.up * rotateX);  // Rotar el jugador (cuerpo) en el eje Y
    }

    // Método que rota las armas del jugador en el eje X (vertical) y limita la rotación
    void RotateArms(float rotateY)
    {
        // Limitar la rotación en el eje X
        xAxisClamp -= rotateY;
        xAxisClamp = Mathf.Clamp(xAxisClamp, -90f, 90f);  // Limitar entre -90 y 90 grados

        // Rotar las armas solo en el eje X (vertical), pero no en el eje Z
        Vector3 playerArmsRotation = playerArms.localRotation.eulerAngles;
        playerArmsRotation.x = xAxisClamp;  // Aplicar la rotación vertical
        playerArmsRotation.z = 0f;          // No permitir rotación en el eje Z (mantener las armas alineadas correctamente)
        playerArms.localRotation = Quaternion.Euler(playerArmsRotation);
    }
}

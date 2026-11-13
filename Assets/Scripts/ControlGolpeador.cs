using UnityEngine;
using UnityEngine.InputSystem;

// Mueve el cilindro con WASD o flechas y golpea la pelota con AddForce.
public class ControlGolpeador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 6f;

    [Header("Golpe")]
    public float fuerzaGolpe = 5f;          // impulso al chocar con la pelota
    public float fuerzaDisparo = 8f;        // impulso del disparo con la barra espaciadora
    public float distanciaDisparo = 1.8f;   // alcance del disparo

    [Header("Referencias")]
    public GestorJuego gestor;
    public Rigidbody pelota;

    private Rigidbody rb;
    private bool disparoSolicitado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current == null || !gestor.juegoActivo)
        {
            return;
        }

        // La tecla se lee en Update para no perder la pulsación
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            disparoSolicitado = true;
        }
    }

    void FixedUpdate()
    {
        // Controles bloqueados si terminó el juego
        if (Keyboard.current == null || !gestor.juegoActivo)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            disparoSolicitado = false;
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            vertical = 1f;
        }
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            vertical = -1f;
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1f;
        }
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1f;
        }

        Vector3 direccion = new Vector3(horizontal, 0f, vertical);

        // Misma rapidez en diagonal
        if (direccion.magnitude > 1f)
        {
            direccion.Normalize();
        }

        rb.linearVelocity = new Vector3(direccion.x * velocidad, rb.linearVelocity.y, direccion.z * velocidad);

        // Disparo con la barra espaciadora si la pelota está cerca
        if (disparoSolicitado)
        {
            disparoSolicitado = false;

            Vector3 haciaPelota = pelota.position - transform.position;
            haciaPelota.y = 0f;

            if (haciaPelota.magnitude <= distanciaDisparo)
            {
                Vector3 empuje = (haciaPelota.normalized + Vector3.up * 0.25f).normalized;
                pelota.AddForce(empuje * fuerzaDisparo, ForceMode.Impulse);
                Debug.Log("Disparo a la pelota");
            }
        }
    }

    // Al chocar con la pelota se le aplica un impulso en dirección contraria al cilindro
    void OnCollisionEnter(Collision colision)
    {
        if (!gestor.juegoActivo)
        {
            return;
        }

        if (colision.gameObject.CompareTag("Pelota"))
        {
            Vector3 direccion = colision.transform.position - transform.position;
            direccion.y = 0f;

            colision.rigidbody.AddForce(direccion.normalized * fuerzaGolpe, ForceMode.Impulse);
            Debug.Log("Golpe a la pelota");
        }
    }
}

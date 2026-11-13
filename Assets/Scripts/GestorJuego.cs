using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// Controla el marcador, la cuenta regresiva de 30 s, el fin del juego y el reinicio con R.
public class GestorJuego : MonoBehaviour
{
    [Header("Objetos")]
    public Rigidbody pelota;
    public Rigidbody golpeador;

    [Header("Interfaz")]
    public TextMeshProUGUI textoMarcador;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoFin;

    [Header("Configuración")]
    public float duracion = 30f;

    // Los demás scripts consultan esta variable para saber si se puede jugar
    public bool juegoActivo = true;

    private int goles = 0;
    private float tiempoRestante;
    private Vector3 posInicialPelota;
    private Vector3 posInicialGolpeador;

    void Start()
    {
        // Se guardan las posiciones iniciales para poder reiniciar
        posInicialPelota = pelota.transform.position;
        posInicialGolpeador = golpeador.transform.position;
        IniciarPartida();
    }

    void Update()
    {
        // R reinicia la partida en cualquier momento
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            IniciarPartida();
            return;
        }

        if (!juegoActivo)
        {
            return;
        }

        // Cuenta regresiva con Time.deltaTime
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            TerminarJuego();
        }

        // Si la pelota se cae de la cancha, vuelve al centro (sin sumar gol)
        if (pelota.transform.position.y < -3f)
        {
            ReiniciarPelota();
        }

        ActualizarUI();
    }

    // Lo llama DetectorGol cuando la pelota entra al trigger del marco
    public void SumarGol()
    {
        if (!juegoActivo)
        {
            return;
        }

        goles++;
        ReiniciarPelota();
        ActualizarUI();
    }

    // Devuelve la pelota a su posición inicial y le quita toda la velocidad
    public void ReiniciarPelota()
    {
        pelota.linearVelocity = Vector3.zero;
        pelota.angularVelocity = Vector3.zero;
        pelota.transform.position = posInicialPelota;
    }

    void IniciarPartida()
    {
        goles = 0;
        tiempoRestante = duracion;
        juegoActivo = true;

        textoFin.gameObject.SetActive(false);

        ReiniciarPelota();
        golpeador.linearVelocity = Vector3.zero;
        golpeador.transform.position = posInicialGolpeador;

        ActualizarUI();
    }

    void TerminarJuego()
    {
        juegoActivo = false;
        textoFin.text = "Fin del juego\nGoles: " + goles + "\nPresiona R para reiniciar";
        textoFin.gameObject.SetActive(true);
    }

    void ActualizarUI()
    {
        textoMarcador.text = "Goles: " + goles;
        textoTiempo.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante);
    }
}

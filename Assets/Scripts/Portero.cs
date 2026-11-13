using UnityEngine;

// Punto extra: portero que se mueve de lado a lado frente al marco.
// Requiere Rigidbody con Is Kinematic activado.
public class Portero : MonoBehaviour
{
    public float amplitud = 2f;    // recorrido hacia cada lado
    public float velocidad = 1.5f;

    private Rigidbody rb;
    private Vector3 posInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posInicial = transform.position;
    }

    void FixedUpdate()
    {
        // PingPong va de 0 a amplitud*2 y regresa; se le resta amplitud para centrarlo
        float x = Mathf.PingPong(Time.time * velocidad, amplitud * 2f) - amplitud;
        rb.MovePosition(new Vector3(posInicial.x + x, posInicial.y, posInicial.z));
    }
}

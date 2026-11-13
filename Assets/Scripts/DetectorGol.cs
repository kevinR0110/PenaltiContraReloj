using UnityEngine;

// Va en el objeto ZonaGol (collider con Is Trigger activado, dentro del marco).
public class DetectorGol : MonoBehaviour
{
    public GestorJuego gestor;

    void OnTriggerEnter(Collider otro)
    {
        // Solo cuenta si el objeto que entra tiene el Tag "Pelota"
        if (otro.CompareTag("Pelota"))
        {
            Debug.Log("¡GOL!");
            gestor.SumarGol();
        }
    }
}

using UnityEngine;
using TMPro;

public class obtenerPuntos : MonoBehaviour
{
    // Puntaje
    [SerializeField] private int puntaje;
    // Texto de puntaje
    [SerializeField] private TMP_Text textoPuntaje;

    void Awake()
    {
        puntaje = PlayerPrefs.GetInt("puntos", 0);
        textoPuntaje.text = puntaje.ToString();
    }

    public void OnTriggerEnter(Collider other)
    {
        // Si el objeto colisiona con un punto, lo destruye y obtiene 10 puntos
        if (other.gameObject.tag == "puntos")
        {
            Destroy(other.gameObject);
            // Ganar puntos
            puntaje += 10;
            // Actualizar texto de puntaje
            textoPuntaje.text = puntaje.ToString();
        }
    }

    public void ReiniciarPuntos()
    {
        puntaje = 0;
    }
}

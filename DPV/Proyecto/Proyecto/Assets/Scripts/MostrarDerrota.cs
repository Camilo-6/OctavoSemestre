using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MostrarDerrota : MonoBehaviour
{
    // Texto de la puntacion
    [SerializeField] private TMP_Text textoPuntacion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Obtenemos la puntacion
        int puntacion = PlayerPrefs.GetInt("Puntacion", 0);
        // Mostramos la puntacion
        textoPuntacion.text = "Tu puntuacion es: " + puntacion.ToString();
    }

    // Metodo para volver al menu principal
    public void VolverAMenuPrincipal()
    {
        // Volvemos al menu principal
        SceneManager.LoadScene("MenuPrincipal");
    }

    // Metodo para reiniciar el nivel
    public void ReiniciarNivel()
    {
        // Obtenemos la escena del nivel
        string escenaNivel = PlayerPrefs.GetString("Escena Nivel", "Null");
        if (escenaNivel != "Null")
        {
            // Cargamos la escena del nivel
            SceneManager.LoadScene(escenaNivel);
        }
        else
        {
            // Si no hay escena del nivel, volvemos al menu principal
            VolverAMenuPrincipal();
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class managerCambioEscena : MonoBehaviour
{
    // Cambiar de escena
    public void iniciarJuego()
    {
        Debug.Log("Iniciar juego");
        SceneManager.LoadScene("NuevaEscena");
    }

    // Salir del juego
    public void salirJuego()
    {
        Application.Quit();
    }
}
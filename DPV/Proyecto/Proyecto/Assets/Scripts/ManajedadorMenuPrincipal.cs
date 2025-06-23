using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ManejadorMenu : MonoBehaviour
{
    // Nombre del juego
    [SerializeField] private string nombreJuego = "Vaqueritos contra Aliens";
    // Texto principal
    [SerializeField] private TMP_Text textoPrincipal;
    // Boton de seleccionar nivel
    [SerializeField] private GameObject botonSeleccionarNivel;
    // Boton de ver puntaciones
    [SerializeField] private GameObject botonVerPuntaciones;
    // Boton del tutorial
    [SerializeField] private GameObject botonTutorial;
    // Boton de salir del juego
    [SerializeField] private GameObject botonSalirJuego;
    // Boton para regresar al menu principal
    [SerializeField] private GameObject botonRegresarMenu;
    // Boton del nivel 1
    [SerializeField] private GameObject botonNivel1;
    // Boton del nivel 2
    [SerializeField] private GameObject botonNivel2;
    // Estado del menu, 0 significa que se va a seleccionar un nivel mientras que 1 significa que se va a ver las puntaciones y 2 el tutorial.
    private int estado;

    void Awake()
    {
        // Mostramos el menu principal
        textoPrincipal.gameObject.SetActive(true);
        botonSeleccionarNivel.SetActive(true);
        botonVerPuntaciones.SetActive(true);
        botonTutorial.SetActive(true);
        botonSalirJuego.SetActive(true);
        botonNivel1.SetActive(false);
        botonNivel2.SetActive(false);
        botonRegresarMenu.SetActive(false);
        // Asignar el texto del nombre del juego
        textoPrincipal.text = nombreJuego;
        // Asignar los botones de la escena
        botonSeleccionarNivel.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SeleccionarNivel);
        botonVerPuntaciones.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(VerPuntaciones);
        botonTutorial.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(MostrarTutorial);
        botonSalirJuego.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SalirJuego);
        botonRegresarMenu.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(RegresarMenu);
        botonNivel1.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => PasarEscena("Nivel1"));
        botonNivel2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => PasarEscena("Nivel2")); // TODO: implementar el nivel 2
        // Actualizamos el estado del menu
        estado = -1;
    }

    // Metodo para seleccionar un nivel
    private void SeleccionarNivel()
    {
        // Cambiamos el estado del menu
        estado = 0;
        // Ocultamos los botones del menu principal
        botonSeleccionarNivel.SetActive(false);
        botonVerPuntaciones.SetActive(false);
        botonTutorial.SetActive(false);
        botonSalirJuego.SetActive(false);
        // Mostramos los botones de los niveles
        botonNivel1.SetActive(true);
        botonNivel2.SetActive(true);
        botonRegresarMenu.SetActive(true);
        // Actualizamos el texto principal
        textoPrincipal.text = "Selecciona un nivel";
    }

    // Metodo para ver las puntaciones
    private void VerPuntaciones()
    {
        // Cambiamos el estado del menu
        estado = 1;
        // Ocultamos los botones del menu principal
        botonSeleccionarNivel.SetActive(false);
        botonVerPuntaciones.SetActive(false);
        botonTutorial.SetActive(false);
        botonSalirJuego.SetActive(false);
        // Mostramos los botones de los niveles
        botonNivel1.SetActive(true);
        botonNivel2.SetActive(true);
        botonRegresarMenu.SetActive(true);
        // Actualizamos el texto principal
        textoPrincipal.text = "Selecciona un nivel";
    }

    private void MostrarTutorial()
    {
        //Cambiamos el estado del menú
        estado = 2;
        PasarEscena("Tutorial"); 
    }

    // Metodo para salir del juego
    private void SalirJuego()
    {
        // Salimos del juego
        Application.Quit();
    }

    // Metodo para regresar al menu principal
    private void RegresarMenu()
    {
        // Volvemos al menu principal
        estado = -1; // Resetear el estado
        textoPrincipal.gameObject.SetActive(true);
        textoPrincipal.text = nombreJuego;
        botonSeleccionarNivel.SetActive(true);
        botonVerPuntaciones.SetActive(true);
        botonTutorial.SetActive(true);
        botonSalirJuego.SetActive(true);
        botonNivel1.SetActive(false);
        botonNivel2.SetActive(false);
        botonRegresarMenu.SetActive(false);
    }

    // Metodo para pasar a la escena del nivel seleccionado
    private void PasarEscena(string nombreEscena)
    {
        // Guardamos el nombre de la escena en PlayerPrefs
        PlayerPrefs.SetString("Escena Nivel", nombreEscena);

        // Revisamos el estado para determinar qué acción tomar
        switch (estado)
        {
            case 0: // Niveles
                SceneManager.LoadScene(nombreEscena);
                break;
            case 1: // Puntuaciones
                SceneManager.LoadScene("Puntuaciones");
                break;
            case 2: // Tutorial
                SceneManager.LoadScene("Tutorial");
                break;
            default:
                Debug.LogWarning("Estado no reconocido: " + estado);
                break;
        }
    }
}

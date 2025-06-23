using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class MostrarVictoria : MonoBehaviour
{
    // Texto ganaste
    [SerializeField] private TMP_Text textoGanaste;
    // Texto de la puntacion
    [SerializeField] private TMP_Text textoPuntacion;
    // Boton de volver al menu principal
    [SerializeField] private GameObject botonVolverMenuPrincipal;
    // Boton de mostrar puntaciones
    [SerializeField] private GameObject botonMostrarPuntaciones;
    // Boton para agregar la puntacion
    [SerializeField] private GameObject botonAgregarPuntacion;
    // Texto de nueva puntacion
    [SerializeField] private TMP_Text textoNuevaPuntacion;
    // Texto de ingresa tu nombre
    [SerializeField] private TMP_Text textoIngresaTuNombre;
    // Texto para el nombre del jugador
    [SerializeField] private TMP_InputField inputNombreJugador;
    // Boton de agregar la puntacion
    [SerializeField] private GameObject botonAgregar;
    // Manejador de puntaciones
    // Texto de listo
    [SerializeField] private TMP_Text textoListo;
    [SerializeField] private ManajadorPuntaciones manejadorPuntaciones;
    // Nombre del archivo de puntaciones
    private string archivo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Obtenemos el nivel actual
        string escenaNivel = PlayerPrefs.GetString("Escena Nivel", "Nivel1");
        // Obtenemos el nombre del archivo
        archivo = Path.Combine(Application.persistentDataPath, "puntuaciones_" + escenaNivel + ".json");
        // Obtenemos la puntacion
        int puntacion = PlayerPrefs.GetInt("Puntacion", 0);
        // Mostramos el texto de ganaste
        string nivel = escenaNivel.Replace("Nivel", "");
        textoGanaste.text = "Lograste pasar el nivel " + nivel + "!";
        // Mostramos la puntacion
        textoPuntacion.text = "Tu puntuacion es: " + puntacion.ToString();
        // Revisamos si la puntacion entra en el top 5
        if (manejadorPuntaciones.RevisarPuntacion(puntacion, archivo))
        {
            // Si entra, mostramos el boton para agregar la puntacion
            botonAgregarPuntacion.SetActive(true);
            // Ocultamos el boton para ver las puntaciones
            botonMostrarPuntaciones.SetActive(false);
            // Ocultamos el boton de volver al menu principal
            botonVolverMenuPrincipal.SetActive(false);
            // Mostramos el texto para agregar la puntacion
            textoNuevaPuntacion.gameObject.SetActive(true);
        }
    }

    // Metodo para pedir el nombre del jugador
    public void PedirNombreJugador()
    {
        // Mostramos el texto para ingresar el nombre
        textoIngresaTuNombre.gameObject.SetActive(true);
        // Mostramos el input para ingresar el nombre
        inputNombreJugador.gameObject.SetActive(true);
        // Mostramos el boton para agregar la puntacion
        botonAgregar.SetActive(true);
        // Ocultamos el texto de ganaste
        textoGanaste.gameObject.SetActive(false);
        // Ocultamos el texto de la puntacion
        textoPuntacion.gameObject.SetActive(false);
        // Ocultamos el boton para agregar la puntacion
        botonAgregarPuntacion.SetActive(false);
        // Ocultamos el texto para agregar la puntacion
        textoNuevaPuntacion.gameObject.SetActive(false);
    }

    // Metodo para agregar una puntacion
    public void AgregarPuntacion()
    {
        // Obtenemos el nombre del jugador
        string nombreJugador = inputNombreJugador.text;
        if (string.IsNullOrEmpty(nombreJugador))
        {
            nombreJugador = "Jugador";
        }
        // Obtenemos la puntacion
        int puntacion = PlayerPrefs.GetInt("Puntacion", 0);
        // Guardamos la puntacion
        manejadorPuntaciones.GuardarPuntacion(nombreJugador, puntacion, archivo);
        // Mostramos el boton para ver las puntaciones
        botonMostrarPuntaciones.SetActive(true);
        // Mostramos el boton de volver al menu principal
        botonVolverMenuPrincipal.SetActive(true);
        // Mostramos el texto de listo
        textoListo.gameObject.SetActive(true);
        // Ocultamos el texto para ingresar el nombre
        textoIngresaTuNombre.gameObject.SetActive(false);
        // Ocultamos el input para ingresar el nombre
        inputNombreJugador.gameObject.SetActive(false);
        // Ocualtamos el boton para agregar la puntacion
        botonAgregar.SetActive(false);
    }

    // Metodo para volver al menu principal
    public void VolverAMenuPrincipal()
    {
        // Volvemos al menu principal
        SceneManager.LoadScene("MenuPrincipal");
    }

    // Metodo para mostrar las puntaciones
    public void MostrarPuntaciones()
    {
        // Pasamos a la escena de puntaciones
        SceneManager.LoadScene("Puntuaciones");
    }
}

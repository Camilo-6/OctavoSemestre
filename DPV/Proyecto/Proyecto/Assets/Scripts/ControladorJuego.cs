using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorJuego : MonoBehaviour
{
    // Controlador de rondas/enemigos
    [SerializeField] private SpawnerEnemigos spawnerEnemigos;
    // Controlador de recursos
    [SerializeField] private RecursoSistema recursoSistema;
    // Cantidad de vidas totales
    [SerializeField] private int vidasTotales;
    // Cantidad de vidas actuales
    private int vidasActuales;
    // Boton para iniciar las rondas
    [SerializeField] private GameObject botonIniciar;
    [SerializeField] private GameObject botonRegresar;

    void Awake()
    {
        // Actualizar las vidas actuales
        vidasActuales = vidasTotales;
        // Darle al boton on click el metodo IniciarNivel
        botonIniciar.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(IniciarNivel);
        botonRegresar.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(RegresarAMenuPrincipal);
    }

    // Metodo para pasar a la escena de victoria
    public void PasarAVictoria()
    {
        int puntacion = ObtenerPuntacion();
        PlayerPrefs.SetInt("Puntacion", puntacion);
        PlayerPrefs.SetString("Escena Nivel", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Victoria");
    }

    // Metodo para pasar a la escena de derrota
    public void PasarADerrota()
    {
        int puntacion = ObtenerPuntacion();
        PlayerPrefs.SetInt("Puntacion", puntacion);
        PlayerPrefs.SetString("Escena Nivel", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Derrota");
    }

    private void Perder()
    {
        PasarADerrota();
    }

    private void Ganar()
    {
        PasarAVictoria();
    }

    // Metodo para obtener la puntacion del nivel
    public int ObtenerPuntacion()
    {
        // Obtenemos los recursos actuales
        int[] recursos = recursoSistema.GetRecursosDisponibles();
        // Obtener la ronda actual
        int rondaActual = spawnerEnemigos.GetRondaActual();
        // Hacer un calculo de la puntacion
        // Multiplicamos los clavos por 5
        int clavos = recursos[0] * 5;
        // Multiplicamos la madera por 10
        int madera = recursos[1] * 10;
        // Multiplicamos las monades por 15
        int monadas = recursos[2] * 15;
        // Sumamos todo
        int puntacion = clavos + madera + monadas;
        // Multiplicamos por la ronda actual
        puntacion *= rondaActual;
        // Multiplicamos por las vidas actuales
        if (vidasActuales <= 0)
        {
            // Evitar multiplicar por cero
            vidasActuales = 1;
        }
        puntacion *= vidasActuales;
        // Retornamos la puntacion
        return puntacion;
    }

    // Metodo para disminuir las vidas
    public void DisminuirVidas(int cantidad)
    {
        vidasActuales -= cantidad;
        if (vidasActuales <= 0)
        {
            vidasActuales = 0;
            Perder();
        }
    }

    // Metodo para cuando todos los enemigos del nivel han sido eliminados
    public void OnAllLevelEnemiesCleared()
    {
        if (vidasActuales > 0)
        {
            Ganar();
        }
    }

    // Metodo para obtener las vidas actualaes
    public int getVidasActuales()
    {
        return vidasActuales;
    }

    // Metodo para obtener las vidas totales
    public int getVidasTotales()
    {
        return vidasTotales;
    }

    // Metodo para obtener la ronda actual
    public int getRondaActual()
    {
        return spawnerEnemigos.GetRondaActual();
    }

    // Metodo para obtener el total de rondas
    public int getTotalRondas()
    {
        return spawnerEnemigos.GetTotalRondas();
    }

    // Metodo para iniciar el nivel
    public void IniciarNivel()
    {
        // Obtenemos todos los generadores de recursos
        GeneradorRecurso[] generadoresRecursos = FindObjectsByType<GeneradorRecurso>(FindObjectsSortMode.None);
        // Activar todos los generadores de recursos
        foreach (GeneradorRecurso generador in generadoresRecursos)
        {
            generador.ActivarGenerador();
        }
        // Desactivar el boton de inicio
        botonIniciar.SetActive(false);
        botonRegresar.SetActive(false);
        // Iniciar el spawner de enemigos
        spawnerEnemigos.Iniciar();
    }

    public void RegresarAMenuPrincipal() {
        SceneManager.LoadScene("MenuPrincipal");
    }

}
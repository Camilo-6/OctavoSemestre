using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SpawnerEnemigos : MonoBehaviour
{
    // Controlador de juego
    [SerializeField] private ControladorJuego controladorJuego;
    // Prefabs de enemigos
    [SerializeField] private GameObject prefabEnemigoBasico;
    [SerializeField] private GameObject prefabEnemigoRapido;
    [SerializeField] private GameObject prefabEnemigoPesado;
    [SerializeField] private GameObject prefabEnemigoBasico_L2;
    [SerializeField] private GameObject prefabEnemigoRapido_L2;
    [SerializeField] private GameObject prefabEnemigoPesado_L2;
    [SerializeField] private GameObject prefabEnemigoArmadura;
    [SerializeField] private GameObject prefabEnemigoArmaduraEnMuerte;
    [SerializeField] private GameObject prefabEnemigoJefeOvni;
    // Datos de las rondas de enemigos
    [SerializeField] private LevelDataEnemigos levelData;
    // Tiempo de espera inicial antes de iniciar el spawner
    [SerializeField] private float initialDelay = 1f;
    // Texto de ultima ronda
    [SerializeField] private TMP_Text textoUltimaRonda;

    private int rondaActual;
    private List<EnemigoBasico> enemigosActivos = new List<EnemigoBasico>();

    void Start()
    {
        rondaActual = 0;
    }

    // Metodo para iniciar las rondas del spawner de enemigos
    public void Iniciar()
    {
        StartCoroutine(SpawnRounds());
    }

    // Metodo que se llama cuando un enemigo muere
    private void HandleMuerteEnemigo(EnemigoBasico enemigo)
    {
        // Desregistramos el evento de muerte
        enemigo.OnDeath -= HandleMuerteEnemigo;
        // Quitamos el enemigo de la lista de enemigos activos
        enemigosActivos.Remove(enemigo);
        // Si es el ultimo enemigo de la ronda, llamamos a LevelCleared
        if (enemigosActivos.Count == 0 && rondaActual == levelData.rondas.Count)
        {
            LevelCleared();
        }
    }

    // Metodo que se llama cuando un enemigo alcanza el objetivo
    private void HandleEnemigoAlcanzaObjetivo(EnemigoBasico enemigo)
    {
        // Revisamos si el enemigo es un jefe (esta en la layer "Jefe")
        if (enemigo.gameObject.layer == LayerMask.NameToLayer("Jefe"))
        {
            // Obtenemos las vidas actuales del controlador de juego
            int vidasActuales = controladorJuego.getVidasActuales();
            // Perdemos todas las vidas
            controladorJuego.DisminuirVidas(vidasActuales);
            // Quitamos el evento de llegar al objetivo
            enemigo.OnReachTarget -= HandleEnemigoAlcanzaObjetivo;
            return;
        }
        else
        {
            // Perdemos una vida
            controladorJuego.DisminuirVidas(1);
        }
        // Quitamos el evento de llegar al objetivo
        enemigo.OnReachTarget -= HandleEnemigoAlcanzaObjetivo;
        // Quitamos el enemigo de la lista de enemigos activos
        enemigosActivos.Remove(enemigo);
        // Si no hay enemigos activos y hemos completado todas las rondas, llamamos a LevelCleared
        if (enemigosActivos.Count == 0 && rondaActual == levelData.rondas.Count)
        {
            LevelCleared();
        }
    }

    // Metodo para generar un enemigo
    private void GenerarEnemigo(GameObject prefabEnemigo, Transform[] pathPoints)
    {
        // Instanciamos el enemigo en la posicion del spawner
        GameObject enemigoObj = Instantiate(prefabEnemigo, transform.position, Quaternion.identity);
        EnemigoBasico enemigo = enemigoObj.GetComponent<EnemigoBasico>();
        // Agregamos el enemigo a la lista de enemigos activos
        enemigosActivos.Add(enemigo);
        // Inicializamos el enemigo con los puntos del camino
        enemigo.Initialize(pathPoints);
        // Registramos los eventos de muerte y alcanzar objetivo
        enemigo.OnDeath += HandleMuerteEnemigo;
        enemigo.OnReachTarget += HandleEnemigoAlcanzaObjetivo;
    }

    // Corrutina para spawnear las rondas de enemigos
    IEnumerator SpawnRounds()
    {
        // Esperamos un tiempo antes de spawnear los enemigos
        yield return new WaitForSeconds(initialDelay);

        // Obtenemos los puntos del camino
        Transform[] pathPoints = levelData.caminoPrefab.GetComponent<Camino>().puntos;

        // Iteramos sobre las rondas
        foreach (RondaEnemigos round in levelData.rondas)
        {
            rondaActual = rondaActual + 1;

            // Si es la ultima ronda, mostramos el texto de ultima ronda
            if (rondaActual == levelData.rondas.Count)
            {
                StartCoroutine(MostrarTextoUltimaRonda());
            }

            // Generamos los enemigos de la ronda actual
            for (int i = 0; i < round.enemigoBasicoCount; i++)
            {
                GenerarEnemigo(prefabEnemigoBasico, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = 0; i < round.enemigoRapidoCount; i++)
            {
                GenerarEnemigo(prefabEnemigoRapido, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = 0; i < round.enemigoPesadoCount; i++)
            {
                GenerarEnemigo(prefabEnemigoPesado, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }

            for (int i = 0; i < round.enemigoBasicoL2Count; i++)
            {
                GenerarEnemigo(prefabEnemigoBasico_L2, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = 0; i < round.enemigoRapidoL2Count; i++)
            {
                GenerarEnemigo(prefabEnemigoRapido_L2, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = 0; i < round.enemigoPesadoL2Count; i++)
            {
                GenerarEnemigo(prefabEnemigoPesado_L2, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }

            for (int i = 0; i < round.enemigoArmaduraCount; i++)
            {
                GenerarEnemigo(prefabEnemigoArmadura, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = 0; i < round.enemigoArmaduraEnMuerteCount; i++)
            {
                GenerarEnemigo(prefabEnemigoArmaduraEnMuerte, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }

            for (int i = 0; i < round.enemigoJefeOvniCount; i++)
            {
                GenerarEnemigo(prefabEnemigoJefeOvni, pathPoints);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(round.delayAfterRound);
        }
    }

    // Metodo para obtener la ronda actual
    public int GetRondaActual()
    {
        return rondaActual;
    }

    // Metodo para obtener el total de rondas
    public int GetTotalRondas()
    {
        return levelData.rondas.Count;
    }

    // Metodo para avisar al controlador de juego que se han eliminado todos los enemigos de la ronda
    private void LevelCleared()
    {
        Debug.Log("LEVEL CLEARED");
        controladorJuego.OnAllLevelEnemiesCleared();
    }

    // Corrutina para mostrar el texto de la ultima ronda
    IEnumerator MostrarTextoUltimaRonda()
    {
        textoUltimaRonda.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        textoUltimaRonda.gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;

public class GeneradorRecurso : MonoBehaviour
{
    // Torre relacionada
    [SerializeField] private ControladorTorre torre;
    // Sistema de recursos
    [SerializeField] private RecursoSistema sistema;
    // Prefab del recurso
    [SerializeField] private GameObject recursoPrefab;
    // Tiempo de vida del recurso
    [SerializeField] private float recursoLifetime = 30f;
    // Intervalo de spawn
    [SerializeField] private float spawnInterval = 15f;
    // Puntos de spawn
    [SerializeField] private Transform[] puntosSpawn;
    // Punto de spawn
    [SerializeField] private Transform spawnPoint;
    // Temporizador para el spawn
    private float spawnTimer = 0f;
    // Cantidad de recursos generados
    [SerializeField] private int cantidadRecursosGenerados = 0;
    private int cantidadInicialRecursosGenerados;
    // Booleano para verificar si el generador está activo
    private bool activo = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = this.transform;
        torre.OnMejorar += HandleMejorarTorre;
        torre.OnDestruir += HandleDestruirTorre;
        cantidadInicialRecursosGenerados = cantidadRecursosGenerados;
    }

    // Update is called once per frame
    void Update()
    {
        // Si el generador no esta activo, no hacemos nada
        if (!activo) return;
        // Aumentamos el tiempo de spawn
        spawnTimer += Time.deltaTime;
        // // Revisamos si la torres esta activa
        if (torre.GetEstado() == 0) return;
        // Si el temporizador ha alcanzado el intervalo de spawn, generamos un nuevo recurso
        if (spawnTimer >= spawnInterval)
        {
            SpawnRecurso();
            spawnTimer = 0f;
        }
    }

    // Metodo para generar un nuevo recurso
    public void SpawnRecurso()
    {
        if (recursoPrefab == null) return;
        // Obtener un punto de spawn aleatorio
        if (puntosSpawn.Length > 0)
        {
            int randomIndex = Random.Range(0, puntosSpawn.Length);
            spawnPoint = puntosSpawn[randomIndex];
        }
        GameObject nuevoRecurso = Instantiate(recursoPrefab, spawnPoint.position, Quaternion.identity);
        // Inicializamos el recurso con el sistema y tipo de recurso
        Recurso recurso = nuevoRecurso.GetComponent<Recurso>();
        recurso.Initialize(sistema, cantidadRecursosGenerados);
        //RecursoSelfDestruct selfDestruct = nuevoRecurso.AddComponent<RecursoSelfDestrcut>();
        StartCoroutine(SelfDestructAfterTime(nuevoRecurso, recursoLifetime));

        //selfDestruct.Initialize(this, recursoLifetime);
        //Instantiate(recursoPrefab, spawnPoint.position, Quaternion.identity);
    }

    // Corrutina para destruir el recurso despues de un tiempo
    private IEnumerator SelfDestructAfterTime(GameObject resource, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        if (resource != null)
        {
            Destroy(resource);
        }
    }

    // Metodos para manejar el evento de mejora de la torre
    private void HandleMejorarTorre(ControladorTorre torre)
    {
        // Aumentar la cantidad de recursos generados
        cantidadRecursosGenerados += 2; // Aumentar en 2 por mejora, experimental
    }

    // Metodos para manejar el evento de destruccion de la torre
    private void HandleDestruirTorre(ControladorTorre torre)
    {
        cantidadRecursosGenerados = cantidadInicialRecursosGenerados;
    }

    // Activar el generador de recursos
    public void ActivarGenerador()
    {
        activo = true;
    }
}

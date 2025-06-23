using UnityEngine;
using System.Collections;
using TMPro;

public class RecursoSistema : MonoBehaviour
{
    // Tipos de recursos que maneja el sistema
    public enum TipoRecurso { Clavo, Madera, Moneda }

    [System.Serializable]
    // Clase para configurar cada tipo de recurso
    public class RecursoConfig
    {
        public TipoRecurso tipo;
        public int cantidadInicial;
        public TMP_Text textoUI;
    }

    //CONFIGURACION  DE RECURSO
    [Header("Configuracion de Recursos")]
    [SerializeField] private RecursoConfig[] configuracionRecursos;

    //SETTINGS DE COMPORTAMIENTO DE RECURSO
    [Header("Spawn de Recursos")]
    [SerializeField] private GameObject recursoPrefab;

    //APARIENCIA RECURSOS
    [Header("Visuales")]
    //[SerializeField] private SpriteRenderer spriteRenderer;
    // Texto de no hay recursos
    [SerializeField] private TMP_Text textoNoHay;
    // Corrutina para mostrar el texto
    private Coroutine mostrarNoHay;
    // Cantidad de recursos disponibles
    private int[] recursosDisponibles;
    // Booleano para verificar si el sistema esta activo
    private bool sistemaActivo = false;

    // Start is called before the first frame update
    private void Start()
    {
        // Inicializar array de recursos
        recursosDisponibles = new int[System.Enum.GetValues(typeof(TipoRecurso)).Length];
        // Actualizamos la UI para cada tipo de recurso
        foreach (var config in configuracionRecursos)
        {
            recursosDisponibles[(int)config.tipo] = config.cantidadInicial;
            ActualizarUI(config.tipo);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!sistemaActivo) return;
    }

    // Metodo para activar el sistema de recursos
    public void ActivarSistema()
    {
        sistemaActivo = true;
    }

    // Metodo para revisar si hay suficientes recursos dado un costo
    public bool TieneSuficientes(int[] costos)
    {
        if (costos == null || costos.Length > recursosDisponibles.Length) return false;
        // Revisamos si hay suficientes de cada tipo de recurso
        for (int i = 0; i < costos.Length; i++)
        {
            if (recursosDisponibles[i] < costos[i])
                return false;
        }
        return true;
    }

    // Metodo para quitar recursos dados unos costo
    public void GastarRecursos(int[] costos)
    {
        // Verificamos si hay suficientes recursos
        if (!TieneSuficientes(costos)) return;
        // Restamos a cada tipo de recurso
        for (int i = 0; i < costos.Length; i++)
        {
            recursosDisponibles[i] -= costos[i];
            ActualizarUI((TipoRecurso)i);
        }
    }

    // Metodo para agregar recursos al sistema
    public void AgregarRecursos(int[] recursos)
    {
        if (recursos == null) return;
        // Aumentamos cada tipo de recurso
        for (int i = 0; i < recursos.Length && i < recursosDisponibles.Length; i++)
        {
            recursosDisponibles[i] += recursos[i];
            ActualizarUI((TipoRecurso)i);
            //RemoveRecurso(recursos);
        }
    }

    // Metodo para actualizar la UI de los recursos disponibles
    private void ActualizarUI(TipoRecurso tipo)
    {
        foreach (var config in configuracionRecursos)
        {
            if (config.tipo == tipo && config.textoUI != null)
            {
                config.textoUI.text = $"{recursosDisponibles[(int)tipo]}";
            }
        }
    }

    // Metodo para regresar la cantidad de recursos disponibles de un tipo
    public int GetCurrentResources(TipoRecurso tipo)
    {
        return recursosDisponibles[(int)tipo]; //?
    }

    // Metodo para obtener los recursos disponibles
    public int[] GetRecursosDisponibles()
    {
        return (int[])recursosDisponibles.Clone();
    }

    // Metodo para mostrar el texto de no hay recursos
    public void MostrarTexto(float segundos)
    {
        // Si ya hay una corrutina en ejecucion, la detenemos
        if (mostrarNoHay != null)
        {
            StopCoroutine(mostrarNoHay);
        }
        // Iniciamos la corrutina para mostrar el texto
        mostrarNoHay = StartCoroutine(MostrarTextoCoroutine(segundos));
    }

    // Corrutina para mostrar el texto durante un tiempo especificado
    IEnumerator MostrarTextoCoroutine(float segundos)
    {
        // Activamos el texto
        textoNoHay.gameObject.SetActive(true);
        //textoNoHay.color = Color.red;
        // Esperamos el tiempo especificado
        yield return new WaitForSeconds(segundos);
        // Desactivamos el texto
        textoNoHay.gameObject.SetActive(false);
    }

}

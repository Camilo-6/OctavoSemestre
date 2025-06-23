using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class Recurso : MonoBehaviour
{
    // Now using the same enum as SistemaRecurso
    public RecursoSistema.TipoRecurso tipoRecurso; //estanderizar

    [Header("Visuales")]
    // SpriteRenderer del recurso
    [SerializeField] private SpriteRenderer spriteRenderer;
    // Cantidad de recursos que va a dar al recolectar
    [SerializeField] private int cantidad; // How much of this resource to add when collected
    // Collider para detectar clicks
    [SerializeField] private Collider2D colliderR;
    // Sistema de recursos
    private RecursoSistema system;
    //private RecursoSelfDestruct selfDestruct;
    // Booleano para verificar si el recurso ya fue recolectado
    private bool fueRecolectado = false;

    // Constructor que recibe un sistema de recursos y una cantidad
    public void Initialize(RecursoSistema manager, int cantidadRecursos)
    {
        system = manager;
        cantidad = cantidadRecursos;
        //Debug.Log($"Inicializando Recurso {tipoRecurso} con {cantidadRecursos}");
        // Add self-destruct component
        //selfDestruct = gameObject.AddComponent<ResourceSelfDestruct>();
        //selfDestruct.Initialize(system, lifeSpan);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Obtenemos el collider
        var collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.isTrigger = true;
        //Debug.Log("Se creo el resurso: " + gameObject.name + " tiene un sprite renderer con la imagen: " + spriteRenderer.sprite);
    }

    // Update is called once per frame
    void Update()
    {
        // Verificamos si el mouse esta arriba del collider del recurso
        Vector2 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseArriba = colliderR.OverlapPoint(posicionMouse);
        // Si el mouse esta arriba
        if (mouseArriba && !EventSystem.current.IsPointerOverGameObject())
        {
            // Cambiar el color del sprite al pasar el mouse
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.green;
            }
        }
        else
        {
            // Restaurar el color original si no esta arriba
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }
        // Al estar arriba y hacer click, recolectar el recurso
        if (mouseArriba && Input.GetMouseButtonDown(0))
        {
            RecolectarRecurso();
        }
    }

    // Metodo para recolectar el recurso
    private void RecolectarRecurso()
    {
        if (fueRecolectado) return;
        fueRecolectado = true;

        // Notify self-destruct component
        /*if (selfDestruct != null)
        {
            selfDestruct.ResourcePickedUp();
        }*/

        // Disable visuals and collisions
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

        // Add resources to the system
        int[] recursosToAdd = new int[System.Enum.GetValues(typeof(RecursoSistema.TipoRecurso)).Length];
        recursosToAdd[(int)tipoRecurso] = cantidad;
        system.AgregarRecursos(recursosToAdd);

        Destroy(gameObject, 0.1f);
    }

    /*
    private void OnMouseEnter()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
        }
    }

    private void OnMouseExit()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }
    */
}
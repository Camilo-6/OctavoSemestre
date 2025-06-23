using UnityEngine;
using System.Collections;

public class AtaqueOvni : MonoBehaviour
{
    // Torre objetivo
    private GameObject torreObjetivo;
    // Cantidad de clicks necesarios
    [SerializeField] private int cantidadClicks;
    // Tiempo de vida
    [SerializeField] private float tiempoDeVida;
    // Collider del ovni
    [SerializeField] private Collider2D colliderOvni;
    // SpriteRenderer del ovni
    [SerializeField] private SpriteRenderer spriteRendererOvni;
    // Color actual del ovni
    private Color colorActual;
    // Corrutina para destruir el ovni
    private Coroutine corrutinaDestruccion;
    // Estado de la torre objetivo
    private int estadoTorreObjetivo;

    // Update is called once per frame
    void Update()
    {
        // Al hacer click en el ovni
        if (Input.GetMouseButtonDown(0) && colliderOvni.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            DisminuirClicks();
        }
    }

    // Metodo para disminuir la cantidad de clicks
    private void DisminuirClicks()
    {
        cantidadClicks--;
        // Cambiamos el color del ovni al rojo, entre menos clicks queden mas rojo se vuelve
        colorActual = Color.Lerp(Color.white, Color.red, 1 - (float)cantidadClicks / 5);
        spriteRendererOvni.color = colorActual;
        // Si la cantidad de clicks llega a 0, destruimos el ovni
        if (cantidadClicks <= 0)
        {
            DestruirOvni();
        }
    }

    // Metodo para destruir el ovni
    private void DestruirOvni()
    {
        // Si hay una corrutina de destruccion en ejecucion, la detenemos
        if (corrutinaDestruccion != null)
        {
            StopCoroutine(corrutinaDestruccion);
        }
        // Activamos la torre objetivo
        torreObjetivo.SetActive(true);
        ControladorTorre controladorTorre = torreObjetivo.GetComponent<ControladorTorre>();
        if (controladorTorre != null)
        {
            controladorTorre.Activar(estadoTorreObjetivo);
        }
        // Destruimos el ovni
        Destroy(gameObject);
    }

    // Metodo para asignar la torre objetivo
    public void AsignarTorreObjetivo(GameObject torre)
    {
        torreObjetivo = torre;
        // Ponemos el ovni en la posicion de la torre objetivo
        transform.position = torreObjetivo.transform.position;
        // Desactivamos la torre objetivo
        ControladorTorre controladorTorre = torreObjetivo.GetComponent<ControladorTorre>();
        if (controladorTorre != null)
        {
            estadoTorreObjetivo = controladorTorre.Desactivar();
        }
        torreObjetivo.SetActive(false);
        // Ponemos el color del ovni a blanco
        colorActual = Color.white;
        spriteRendererOvni.color = colorActual;
        // Iniciamos la corrutina de destruccion
        corrutinaDestruccion = StartCoroutine(DestruirLuegoDeTiempo());
    }

    // Corrutina para destruir el ovni luego de un tiempo
    IEnumerator DestruirLuegoDeTiempo()
    {
        // Esperamos el tiempo de vida del ovni
        yield return new WaitForSeconds(tiempoDeVida);
        // Destruimos el ovni
        DestruirOvni();
    }
}

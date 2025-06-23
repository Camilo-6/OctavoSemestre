using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControladorTorre : MonoBehaviour
{
    // Estado
    [SerializeField] private int estado;
    // Controlador de recursos
    [SerializeField] private RecursoSistema sistemaRecursos;
    // Torres
    [SerializeField] private GameObject[] torres;
    // Objetivo a defender
    [SerializeField] private GameObject objetivoDefender;
    // Controlador de informacion
    [SerializeField] private ControladorInformacion controladorInformacion;

    // Boton de construir 1
    [SerializeField] private GameObject botonConstruir1;
    // Boton de construir 2
    [SerializeField] private GameObject botonConstruir2;
    // Boton de construir 3
    [SerializeField] private GameObject botonConstruir3;
    // Boton de construir 4
    [SerializeField] private GameObject botonConstruir4;
    // Boton de mejorar
    [SerializeField] private GameObject botonMejorar;
    // Boton de vender
    [SerializeField] private GameObject botonVender;
    // Collider para dar click en la torre
    [SerializeField] private Collider2D colliderTorre;
    // Booleano para saber si estamos mostrando botones
    [SerializeField] private bool mostrandoBotones;
    // Corrutina para construir
    [SerializeField] private Coroutine corrutinaConstruir;
    // Corrutina para mejorar y vender
    [SerializeField] private Coroutine corrutinaMejorarVender;
    // Corrutina para vender
    [SerializeField] private Coroutine corrutinaVender;
    // Evento de mejora de la torre
    public event System.Action<ControladorTorre> OnMejorar;
    // Evento de destruccion de la torre
    public event System.Action<ControladorTorre> OnDestruir;

    // Start is called before the first frame update
    void Start()
    {
        estado = 0;
        // Desactivar los botones de construir
        botonConstruir1.SetActive(false);
        botonConstruir2.SetActive(false);
        botonConstruir3.SetActive(false);
        botonConstruir4.SetActive(false);
        botonMejorar.SetActive(false);
        botonVender.SetActive(false);
        mostrandoBotones = false;
        // Obtener el sistema de recursos
        if (sistemaRecursos == null)
        {
            sistemaRecursos = FindFirstObjectByType<RecursoSistema>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Al hacer click en la torre
        if (Input.GetMouseButtonDown(0) && colliderTorre.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            // Si no estamos mostrando botones
            if (!mostrandoBotones)
            {
                // Mostrar botones
                mostrandoBotones = true;
                MostrarBotones();
            }
        }
    }

    // Construir torre
    public void Construir(int torre)
    {
        if (estado != 0)
        {
            return;
        }
        // Verificar si la torre es valida
        if (torre < 0 || torre >= torres.Length)
        {
            return;
        }
        // Obtener el precio de la torre
        int[] precio = torres[torre].GetComponent<TorreBasica>().GetPrecio();

        // Verificar si hay suficientes recursos *****
        if (!sistemaRecursos.TieneSuficientes(precio))
        {
            sistemaRecursos.MostrarTexto(2f);
            return;
        }

        sistemaRecursos.GastarRecursos(precio);

        // Activar la torre
        torres[0].SetActive(false);
        torres[torre].SetActive(true);
        torres[torre].GetComponent<TorreBasica>().SetTiempoTranscurrido(0);
        torres[torre].GetComponent<TorreBasica>().SetVidaMaxima();
        torres[torre].GetComponent<TorreBasica>().ActualizarBarraVida();
        colliderTorre = torres[torre].GetComponent<TorreBasica>().GetColliderTorre();
        if (corrutinaConstruir != null)
        {
            StopCoroutine(corrutinaConstruir);
        }
        botonConstruir1.SetActive(false);
        botonConstruir2.SetActive(false);
        botonConstruir3.SetActive(false);
        botonConstruir4.SetActive(false);
        mostrandoBotones = false;
        // Cambiar el estado a la torre activa
        estado = torre;
    }

    // Mejorar torre activa
    public void Mejorar()
    {
        if (estado == 0)
        {
            return;
        }
        if (torres[estado].GetComponent<TorreBasica>().SePuedeMejorar() == false)
        {
            return;
        }
        // Obtener el precio de mejora de la torre activa
        int[] precioMejora = torres[estado + 1].GetComponent<TorreBasica>().GetPrecio();

        // Verificar si hay suficientes recursos******
        if (!sistemaRecursos.TieneSuficientes(precioMejora))
        {
            sistemaRecursos.MostrarTexto(2f);
            return;
        }
        // Gastar los recursos necesarios para mejorar
        sistemaRecursos.GastarRecursos(precioMejora);

        // Desactivar la torre activa
        torres[estado].SetActive(false);
        // Activar la torre mejorada
        torres[estado + 1].SetActive(true);
        torres[estado + 1].GetComponent<TorreBasica>().SetTiempoTranscurrido(0);
        torres[estado + 1].GetComponent<TorreBasica>().SetVidaMaxima();
        torres[estado + 1].GetComponent<TorreBasica>().ActualizarBarraVida();
        colliderTorre = torres[estado + 1].GetComponent<TorreBasica>().GetColliderTorre();
        if (corrutinaMejorarVender != null)
        {
            StopCoroutine(corrutinaMejorarVender);
        }
        if (corrutinaVender != null)
        {
            StopCoroutine(corrutinaVender);
        }
        botonMejorar.SetActive(false);
        botonVender.SetActive(false);
        mostrandoBotones = false;
        // Cambiar el estado a la torre mejorada
        estado++;
        // Invocar el evento de mejorar
        OnMejorar?.Invoke(this);
    }

    // Vender torre activa
    public void Vender()
    {
        if (estado == 0)
        {
            return;
        }
        // Obtener el precio de venta de la torre activa
        int[] precioVenta = torres[estado].GetComponent<TorreBasica>().GetPrecioVenta();

        // Agregar el precio de venta al controlador de recursos
        sistemaRecursos.AgregarRecursos(precioVenta);

        // Desactivar la torre activa
        torres[estado].SetActive(false);
        // Cambiar el estado a 0 (sin torre activa)
        estado = 0;
        torres[0].SetActive(true);
        colliderTorre = torres[0].GetComponent<Collider2D>();
        if (corrutinaMejorarVender != null)
        {
            StopCoroutine(corrutinaMejorarVender);
        }
        if (corrutinaVender != null)
        {
            StopCoroutine(corrutinaVender);
        }
        botonMejorar.SetActive(false);
        botonVender.SetActive(false);
        mostrandoBotones = false;
        // Invocar el evento de destruir
        OnDestruir?.Invoke(this);
    }

    // Desactivar torre activa
    public void DesactivarTorre()
    {
        if (estado == 0)
        {
            return;
        }
        // Desactivar la torre activa
        torres[estado].SetActive(false);
        // Cambiar el estado a 0 (sin torre activa)
        estado = 0;
        torres[0].SetActive(true);
        colliderTorre = torres[0].GetComponent<Collider2D>();
        if (corrutinaMejorarVender != null)
        {
            StopCoroutine(corrutinaMejorarVender);
        }
        if (corrutinaVender != null)
        {
            StopCoroutine(corrutinaVender);
        }
        botonMejorar.SetActive(false);
        botonVender.SetActive(false);
        mostrandoBotones = false;
        // Invocar el evento de destruir
        OnDestruir?.Invoke(this);
    }

    // Mostrar botones
    public void MostrarBotones()
    {
        if (estado == 0)
        {
            // Mostrar botones de construir
            corrutinaConstruir = StartCoroutine(MostrarBotonesConstruir());
        }
        else
        {
            if (torres[estado].GetComponent<TorreBasica>().SePuedeMejorar())
            {
                // Mostrar botones de mejorar y vender
                corrutinaMejorarVender = StartCoroutine(MostrarBotonesMejorarYVender());
            }
            else
            {
                // Mostrar boton de vender
                corrutinaVender = StartCoroutine(MostrarBotonVender());
            }
        }
    }

    // Mostrar botones de construir
    IEnumerator MostrarBotonesConstruir()
    {
        // Mostrar botones de construir
        botonConstruir1.SetActive(true);
        botonConstruir2.SetActive(true);
        botonConstruir3.SetActive(true);
        botonConstruir4.SetActive(true);

        // Mostrar la informacion de construccion opcion 1
        controladorInformacion.DesactivarOpciones();
        int[] precio = torres[1].GetComponent<TorreBasica>().GetPrecio();
        string descripcion = torres[1].GetComponent<TorreBasica>().GetDescripcion();
        Sprite imagen = torres[1].GetComponent<TorreBasica>().imagenArma;
        Button boton = botonConstruir1.GetComponent<Button>();
        Color color = boton.image.color;
        controladorInformacion.MostrarInformacionConstruccion1(descripcion, precio[0], precio[1], precio[2], color, imagen);
        // Mostrar la informacion de construccion opcion 2
        precio = torres[4].GetComponent<TorreBasica>().GetPrecio();
        descripcion = torres[4].GetComponent<TorreBasica>().GetDescripcion();
        imagen = torres[4].GetComponent<TorreBasica>().imagenArma;
        boton = botonConstruir2.GetComponent<Button>();
        color = boton.image.color;
        controladorInformacion.MostrarInformacionConstruccion2(descripcion, precio[0], precio[1], precio[2], color, imagen);
        // Mostrar la informacion de construccion opcion 3
        precio = torres[7].GetComponent<TorreBasica>().GetPrecio();
        descripcion = torres[7].GetComponent<TorreBasica>().GetDescripcion();
        imagen = torres[7].GetComponent<TorreBasica>().imagenArma;
        boton = botonConstruir3.GetComponent<Button>();
        color = boton.image.color;
        controladorInformacion.MostrarInformacionConstruccion3(descripcion, precio[0], precio[1], precio[2], color, imagen);
        // Mostrar la informacion de construccion opcion 4
        precio = torres[10].GetComponent<TorreBasica>().GetPrecio();
        descripcion = torres[10].GetComponent<TorreBasica>().GetDescripcion();
        imagen = torres[10].GetComponent<TorreBasica>().imagenArma;
        boton = botonConstruir4.GetComponent<Button>();
        color = boton.image.color;
        controladorInformacion.MostrarInformacionConstruccion4(descripcion, precio[0], precio[1], precio[2], color, imagen);
        // Esperar 2 segundos
        yield return new WaitForSeconds(2f);
        // Desactivar los botones de construir
        botonConstruir1.SetActive(false);
        botonConstruir2.SetActive(false);
        botonConstruir3.SetActive(false);
        botonConstruir4.SetActive(false);
        mostrandoBotones = false;
        corrutinaConstruir = null;
    }

    // Mostrar botones de mejorar y vender
    IEnumerator MostrarBotonesMejorarYVender()
    {
        // Mostrar botones de mejorar y vender
        botonMejorar.SetActive(true);
        botonVender.SetActive(true);
        // Mostrar la informacion de mejora
        controladorInformacion.DesactivarOpciones();
        int[] precio = torres[estado + 1].GetComponent<TorreBasica>().GetPrecio();
        string descripcion = torres[estado + 1].GetComponent<TorreBasica>().GetDescripcion();
        Button boton = botonMejorar.GetComponent<Button>();
        Color color = boton.image.color;
        controladorInformacion.MostrarInformacionMejora(descripcion, precio[0], precio[1], precio[2], color);
        // Mostrar la informacion de venta
        precio = torres[estado].GetComponent<TorreBasica>().GetPrecioVenta();
        boton = botonVender.GetComponent<Button>();
        color = boton.image.color;
        controladorInformacion.MostrarInformacionVenta(precio[0], precio[1], precio[2], color);
        // Esperar 2 segundos
        yield return new WaitForSeconds(2f);
        // Desactivar los botones de mejorar y vender
        botonMejorar.SetActive(false);
        botonVender.SetActive(false);
        mostrandoBotones = false;
        corrutinaMejorarVender = null;
    }

    // Mostrar boton de vender
    IEnumerator MostrarBotonVender()
    {
        // Mostrar boton de vender
        botonVender.SetActive(true);
        // Mostrar la informacion de venta
        controladorInformacion.DesactivarOpciones();
        int[] precio = torres[estado].GetComponent<TorreBasica>().GetPrecioVenta();
        Button boton = botonVender.GetComponent<Button>();
        Color color = boton.image.color;
        controladorInformacion.MostrarInformacionVenta(precio[0], precio[1], precio[2], color);
        // Esperar 2 segundos
        yield return new WaitForSeconds(2f);
        // Desactivar el boton de vender
        botonVender.SetActive(false);
        mostrandoBotones = false;
        corrutinaVender = null;
    }

    // Metodo para obtener el objetivo a defender
    public GameObject GetObjetivoDefender()
    {
        return objetivoDefender;
    }

    // Metodo para obtener el estado de la torre
    public int GetEstado()
    {
        return estado;
    }

    // Metodo para desactivar la torre
    public int Desactivar()
    {
        // Desactivamos todos los botones
        botonConstruir1.SetActive(false);
        botonConstruir2.SetActive(false);
        botonConstruir3.SetActive(false);
        botonConstruir4.SetActive(false);
        botonMejorar.SetActive(false);
        botonVender.SetActive(false);
        mostrandoBotones = false;
        // Desactivamos todas las corrutinas
        if (corrutinaConstruir != null)
        {
            StopCoroutine(corrutinaConstruir);
            corrutinaConstruir = null;
        }
        if (corrutinaMejorarVender != null)
        {
            StopCoroutine(corrutinaMejorarVender);
            corrutinaMejorarVender = null;
        }
        if (corrutinaVender != null)
        {
            StopCoroutine(corrutinaVender);
            corrutinaVender = null;
        }
        // Regresamos el estado de la torre
        return estado;
    }

    // Metodo para activar la torre
    public void Activar(int nuevoEstado)
    {
        // Activamos la torre correspondiente al estado
        if (nuevoEstado != 0)
        {
            estado = nuevoEstado;
            torres[nuevoEstado].SetActive(true);
            colliderTorre = torres[nuevoEstado].GetComponent<TorreBasica>().GetColliderTorre();
        }
        else
        {
            torres[0].SetActive(true);
            colliderTorre = torres[0].GetComponent<Collider2D>();
            estado = 0;
        }
    }
}

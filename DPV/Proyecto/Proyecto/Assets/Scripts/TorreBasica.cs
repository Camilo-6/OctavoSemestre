using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class TorreBasica : MonoBehaviour
{
    // Danio
    [SerializeField] protected int danio;
    // Rango
    [SerializeField] private Collider2D rango;
    // Tiempo de recarga
    [SerializeField] private float recarga;
    // Precio
    [SerializeField] private int[] precio;
    // Precio venta
    [SerializeField] private int[] venta;
    // Booleano para verificar si la torre se puede mejorar
    [SerializeField] private bool sePuedeMejorar;
    // Vida
    [SerializeField] private int vida;
    // Vida maxima
    [SerializeField] private int vidaMaxima;
    // Armadura
    [SerializeField] private int armadura;
    // Descripcion de la torre
    [SerializeField] private string descripcion;
    // Collider de la torre
    [SerializeField] private Collider2D colliderTorre;
    // Controlador de la torre
    [SerializeField] private ControladorTorre controladorTorre;
    // Objetivo a defender
    [SerializeField] protected GameObject objetivoDefender;
    // Disparo
    [SerializeField] protected GameObject disparoPrefab;
    // Controlador de la vida
    [SerializeField] private ControladorVida controladorVida;
    // Tiempo transcurrido
    [SerializeField] private float tiempoTranscurrido;
    // Objetivo enemigo
    [SerializeField] protected GameObject objetivoEnemigo;
    // Enemigos en rango
    [SerializeField] protected List<GameObject> enemigosEnRango;
    [SerializeField] public Sprite imagenArma;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicializar la lista de enemigos en rango
        enemigosEnRango = new List<GameObject>();
        // Inicializar el objetivo
        objetivoEnemigo = null;
        // Inicializar el tiempo transcurrido
        tiempoTranscurrido = 0;
        // Obtener el objetivo a defender
        objetivoDefender = controladorTorre.GetObjetivoDefender();
    }

    // Update is called once per frame
    void Update()
    {
        // Aumentar el tiempo transcurrido
        tiempoTranscurrido += Time.deltaTime;
        // Si el tiempo transcurrido es mayor o igual al tiempo de recarga
        if (tiempoTranscurrido >= recarga)
        {
            // Obtener el objetivo
            ObtenerObjetivo();
            // Si el objetivo enemigo no es nulo    
            if (objetivoEnemigo != null)
            {
                // Atacar al enemigo
                Atacar();
                // Reiniciar el tiempo transcurrido
                tiempoTranscurrido = 0;
            }
            // En otro caso
            else
            {
                // Poner el tiempo transcurrido en tiempo de recarga
                tiempoTranscurrido = recarga;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto que entra en el rango es un enemigo
        if (collision.CompareTag("Enemigo"))
        {
            // Evitamos los colliders de tipo CircleCollider2D
            if (collision is CircleCollider2D)
            {
                return;
            }
            // Agregar el enemigo a la lista de enemigos en rango
            enemigosEnRango.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Si el objeto que sale del rango es un enemigo
        if (collision.CompareTag("Enemigo"))
        {
            // Evitamos los colliders de tipo CircleCollider2D
            if (collision is CircleCollider2D)
            {
                return;
            }
            // Remover el enemigo de la lista de enemigos en rango
            enemigosEnRango.Remove(collision.gameObject);
        }
    }

    // Metodo para obtener al enemigo a atacar
    protected virtual void ObtenerObjetivo()
    {
        // Limpar la lista de enemigos destruidos
        enemigosEnRango = enemigosEnRango.FindAll(e => e != null);
        // Si hay enemigos en rango
        if (enemigosEnRango.Count > 0)
        {
            // Obtener el enemigo mas cercano al objetivo a defender
            float distanciaMinima = Mathf.Infinity;
            foreach (GameObject enemigo in enemigosEnRango)
            {
                // Calcular la distancia entre el enemigo y el objetivo a defender
                float distancia = Vector2.Distance(enemigo.transform.position, objetivoDefender.transform.position);
                // Si la distancia es menor que la minima
                if (distancia < distanciaMinima)
                {
                    // Actualizar la distancia minima y el objetivo
                    distanciaMinima = distancia;
                    objetivoEnemigo = enemigo;
                }
            }
        }
        else
        {
            // Si no hay enemigos en rango, limpiar el objetivo
            objetivoEnemigo = null;
        }
    }

    // Metodo para atacar al enemigo
    protected virtual void Atacar()
    {
        // Instanciar el disparo
        GameObject disparo = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
        // Asignar el objetivo al disparo
        DisparoBasico disparoScript = disparo.GetComponent<DisparoBasico>();
        if (disparoScript != null)
        {
            disparoScript.SetDanio(danio);
            disparoScript.SetObjetivo(objetivoEnemigo);
        }
        // Revisamos si el danio del ataque mato al objetivo
        if (objetivoEnemigo != null)
        {
            EnemigoBasico enemigoScript = objetivoEnemigo.GetComponent<EnemigoBasico>();
            if (enemigoScript != null)
            {
                bool enemigoMuerto = enemigoScript.MoriraLuegoDeAtaque(danio);
                // Si el enemigo muere, quitarlo de la lista de enemigos en rango
                if (enemigoMuerto)
                {
                    enemigosEnRango.Remove(objetivoEnemigo);
                    objetivoEnemigo = null;
                }
            }
        }
    }

    // Metodo para recibir danio
    public void RecibirDanio(int danio)
    {
        // Si la vida es menor o igual a 0, no recibir danio
        if (vida <= 0)
        {
            return;
        }
        // Calcular el danio a recibir
        int danioReal = danio - armadura;
        // Si el danio a recibir es menor a 0, recibir uno de danio
        if (danioReal < 0)
        {
            danioReal = 1;
        }
        // Restar el danio a recibir a la vida
        vida -= danioReal;
        // Actualizar la barra de vida
        controladorVida.ActualizarVida(vida, vidaMaxima);
        // Si la vida es menor o igual a 0, el enemigo muere
        if (vida <= 0)
        {
            // Avisar al controlador de la torre que la torre ha muerto
            controladorTorre.DesactivarTorre();
        }
    }

    // Metodo para obtener el precio
    public int[] GetPrecio()
    {
        return precio;
    }

    // Metodo para obtener el precio de venta
    public int[] GetPrecioVenta()
    {
        return venta;
    }

    // Metodo para actualizar el tiempo transcurrido
    public void SetTiempoTranscurrido(float tiempo)
    {
        tiempoTranscurrido = tiempo;
    }

    // Metodo para obtener si la torre se puede mejorar
    public bool SePuedeMejorar()
    {
        return sePuedeMejorar;
    }

    // Metodo para obtener el rango de la torre
    public Collider2D GetRango()
    {
        return rango;
    }

    // Metodo para obtener el collider de la torre
    public Collider2D GetColliderTorre()
    {
        return colliderTorre;
    }

    // Metodo para poner una torre en su vida maxima
    public void SetVidaMaxima()
    {
        vida = vidaMaxima;
    }

    // Metodo para actualizar la barra de vida
    public void ActualizarBarraVida()
    {
        controladorVida.ActualizarVida(vida, vidaMaxima);
    }

    // Metodo para obtener la descripcion de la torre
    public string GetDescripcion()
    {
        return descripcion;
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemigoBasico : MonoBehaviour
{
    // Danio
    [SerializeField] private int danio;
    // Vida
    [SerializeField] private int vida;
    // Vida maxima
    [SerializeField] private int vidaMaxima;
    // Armadura
    [SerializeField] private int armadura;


    // Puntos del camino a seguir
    [SerializeField] private Transform[] camino;
    // Punto actual
    [SerializeField] private int puntoActual;
    // Velocidad actual
    [SerializeField] private float velocidadActual;
    // Velocidad inicial
    [SerializeField] private float velocidadInicial;
    // Ancho del camino / 2
    [SerializeField] private float pathOffsetDistance;
    // Distancia maxima del offset del camino
    private float maxPathOffsetDistance = 0.5f;
    // Progreso del camino
    private float pathProgress = 0.0f;



    // Estado
    [SerializeField] protected int estado;
    // Collider del enemigo
    [SerializeField] private Collider2D colliderEnemigo;
    // Controlador de la vida
    [SerializeField] private ControladorVida controladorVida;

    // Prefab del disparo
    [SerializeField] private GameObject disparoPrefab;
    // Tiempo transcurrido para recargar el disparo
    [SerializeField] private float tiempoTranscurridoParaRecarga;
    // Tiempo de recarga del disparo
    [SerializeField] private float tiempoDeRecarga;
    // Número del 0 al 1
    //[SerializeField] private float probabilidadDeAtaque;
    // Maxima duracion de ataque
    // Número del 0 al 1
    [SerializeField] private float maxDuracionDeAtaque;
    // Minima duracion de ataque
    private float minDuracionDeAtaque = 2;
    // Tiempo atacando
    private float tiempoAtacando;

    // Objetivo enemigo
    [SerializeField] private GameObject objetivoTorre;
    // Evento de muerte
    public event System.Action<EnemigoBasico> OnDeath;
    // Evento de alcanzar el objetivo
    public event System.Action<EnemigoBasico> OnReachTarget;

    // Casco del enemigo
    [SerializeField] private GameObject casco;
    // Boolean para saber si el enemigo tiene casco
    [SerializeField] private bool tieneCasco = true;

    public void Initialize(Transform[] camino)
    {
        this.camino = camino;
    }


    void Start()
    {
        objetivoTorre = null;
        tiempoAtacando = 0;
        tiempoTranscurridoParaRecarga = 0;

        velocidadActual = velocidadInicial;
        pathOffsetDistance = Random.Range(-1.0f * maxPathOffsetDistance, maxPathOffsetDistance);
    }

    void Update()
    {
        // Si estamos en el estado 0, mover el objeto
        if (estado == 0)
        {
            // Si hay un objetivo torre, atacar
            if (objetivoTorre != null)
            {
                Atacar();
            }
            // En otro caso, caminar
            else
            {
                MoverseEnParaleloAlCamino();
            }
        }
    }

    // Metodo para mover el enemigo en paralelo al camino
    protected void MoverseEnParaleloAlCamino()
    {
        if (camino.Length < 2) return; // Necesitamos al menos 2 puntos

        pathProgress += velocidadActual * 0.1f * Time.deltaTime;
        // Esta línea hace que se detenga en el último punto. Es importante para que funcione el cálculo del camino
        pathProgress = Mathf.Clamp(pathProgress, 0, camino.Length - 1);

        Vector3 positionOnPath = GetCatmullRomPosition(pathProgress);
        Vector3 tangent = GetCatmullRomTangent(pathProgress);
        Vector3 offsetDir = Vector3.Cross(tangent, Vector3.forward).normalized;

        transform.position = positionOnPath + offsetDir * pathOffsetDistance;
        transform.up = tangent;
    }

    // Metodo para obtener una posicion en la curva Catmull-Rom
    Vector3 GetCatmullRomPosition(float t)
    {
        int numSegments = camino.Length - 1;
        int currentIndex = Mathf.Min(Mathf.FloorToInt(t), numSegments - 1);
        float segmentProgress = t - currentIndex;

        // 4 puntos de control
        Vector3 p0 = GetClampedOrVirtualPoint(currentIndex - 1);
        Vector3 p1 = GetClampedOrVirtualPoint(currentIndex);
        Vector3 p2 = GetClampedOrVirtualPoint(currentIndex + 1);
        Vector3 p3 = GetClampedOrVirtualPoint(currentIndex + 2);

        return CalculateCatmullRom(p0, p1, p2, p3, segmentProgress);
    }

    // Metodo para obtener un punto, incluyendo puntos virtuales para bordes
    Vector3 GetClampedOrVirtualPoint(int index)
    {
        // Puntos virtuales para el final y el inicio del camino
        if (index < 0)
            return camino[0].position * 2 - camino[1].position; // para el 1er punto

        if (index >= camino.Length)
            return camino[camino.Length - 1].position * 2 - camino[camino.Length - 2].position; // para el último punto

        return camino[index].position;
    }

    // Metodo para calcular una posicion usando Catmull-Rom
    Vector3 CalculateCatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Fórmula Catmull-Rom
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    // Metodo para obtener una tangente
    Vector3 GetCatmullRomTangent(float t)
    {
        float delta = 0.001f;
        Vector3 pos1 = GetCatmullRomPosition(t);
        Vector3 pos2 = GetCatmullRomPosition(t + delta);
        return (pos2 - pos1).normalized;
    }

    // Metodo para que el enemigo reciba danio
    public void RecibirDanio(int danio)
    {
        // Si la vida es menor o igual a 0, no recibir daño
        if (vida <= 0)
        {
            return;
        }
        // Calcular el danio a recibir
        int danioReal = danio - armadura;
        // Si el danio a recibir es menor a 0, no recibir daño
        if (danioReal < 0)
        {
            danioReal = 0;
        }
        // Restar el danio a recibir a la vida
        vida -= danioReal;
        // Actualizar la barra de vida
        controladorVida.ActualizarVida(vida, vidaMaxima);
        // Si la vida es menor o igual a 0, el enemigo muere
        if (vida <= 0)
        {
            Morir();
        }
    }

    // Metodo para saber si el enemigo morira luego de un ataque
    public bool MoriraLuegoDeAtaque(int danio)
    {
        // Si la vida es menor o igual a 0, el enemigo ya esta muerto
        if (vida <= 0)
        {
            return true;
        }
        // Calcular el danio a recibir
        int danioReal = danio - armadura;
        // Si el danio a recibir es menor a 0, no recibir danio
        if (danioReal < 0)
        {
            danioReal = 0;
        }
        // Si la vida menos el danio real es menor o igual a 0, el enemigo morira
        return vida - danioReal <= 0;
    }

    // Metodo para cuando se alcanza el objetivo
    public void AlcanzarObjetivo()
    {
        // Invocar el evento de alcanzar el objetivo
        OnReachTarget?.Invoke(this);
        // Destruir el objeto enemigo
        Destroy(gameObject);
    }

    // Metodo para realizar acciones cuando el enemigo muere
    private void Morir()
    {
        // Cambiar el estado a 1 (muerto)
        estado = 1;
        // Invocar el evento de muerte
        OnDeath?.Invoke(this);
        // Realizar acciones adicionales al morir
        AlMorir();
        // Destruir el objeto enemigo
        Destroy(gameObject);
    }

    // Metodo para realizar acciones al morir
    protected virtual void AlMorir()
    {
    }

    // Metodo para asignar una nueva torre objetivo y reiniciar los tiempos atacando y recargando
    private void AsignarNuevaTorreObjetivo(GameObject torre)
    {
        objetivoTorre = torre;
        tiempoAtacando = 0;
        tiempoTranscurridoParaRecarga = 0;
    }

    // Metodo para limpiar el objetivo de la torre y reiniciar los tiempos atacando y recargando
    private void ClearTorreObjetivo()
    {
        objetivoTorre = null;
        tiempoAtacando = 0;
        tiempoTranscurridoParaRecarga = 0;
    }

    // Metodo para atacar a la torre objetivo
    private void Atacar()
    {
        // tiempo atacando
        tiempoAtacando += Time.deltaTime;

        // Manejo de disparos
        // Actualizar el tiempo transcurrido
        tiempoTranscurridoParaRecarga += Time.deltaTime;
        // Si el tiempo transcurrido es mayor o igual al tiempo de recarga, disparar
        if (tiempoTranscurridoParaRecarga >= tiempoDeRecarga)
        {
            // Instanciar el disparo, asignar el objetivo y danio
            GameObject disparo = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
            DisparoEnemigoBasico disparoScript = disparo.GetComponent<DisparoEnemigoBasico>();
            if (disparoScript != null)
            {
                disparoScript.SetObjetivo(objetivoTorre);
                disparoScript.SetDanio(danio);
            }
            // Reiniciar el tiempo transcurrido para recarga
            tiempoTranscurridoParaRecarga = 0;
        }
        // Dejar de atacar si ha pasado cierto tiempo
        if (tiempoAtacando > minDuracionDeAtaque && tiempoAtacando >= maxDuracionDeAtaque)
        {
            ClearTorreObjetivo();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto colisionado es una torre y no hay objetivo torre asignado
        if (collision.CompareTag("Torre") && objetivoTorre == null)
        {
            // Evitamos los colliders de tipo CircleCollider2D
            if (collision is CircleCollider2D) return;
            // Asignar la nueva torre objetivo
            AsignarNuevaTorreObjetivo(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Si el objeto dejado de colisionar es una torre
        if (collision.CompareTag("Torre"))
        {
            // Evitamos los colliders de tipo CircleCollider2D
            if (collision is CircleCollider2D) return;
            // Limpiar el objetivo de la torre
            ClearTorreObjetivo();
        }
    }

    // Metodo para reducir la armadura del enemigo
    public void ReducirArmadura(int cantidad)
    {
        armadura -= cantidad;
        // Evitar que la armadura sea negativa
        if (armadura <= 0)
        {
            armadura = 0;
            // Si la armadura es 0 y el enemigo tiene casco, quitar el casco
            if (tieneCasco)
            {
                QuitarCasco();
                tieneCasco = false;
            }
        }
    }

    // Metodo para agregar armadura al enemigo
    public void AgregarArmadura(int cantidad)
    {
        armadura += cantidad;
        // Si el enemgio no tiene casco, poner el casco
        if (!tieneCasco)
        {
            PonerCasco();
            tieneCasco = true;
        }
    }

    // Metodo para obtener la armadura del enemigo
    public int ObtenerArmadura()
    {
        return armadura;
    }

    // Metodo para quitar el casco del enemigo
    public void QuitarCasco()
    {
        if (casco != null)
        {
            casco.SetActive(false);
        }
    }

    // Metodo para poner el casco del enemigo
    public void PonerCasco()
    {
        if (casco != null)
        {
            casco.SetActive(true);
        }
    }
}

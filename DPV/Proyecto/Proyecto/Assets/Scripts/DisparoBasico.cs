using UnityEngine;

public class DisparoBasico : MonoBehaviour
{
    // Velocidad
    [SerializeField] private float velocidad;
    // Danio
    [SerializeField] protected int danio;
    // Objetivo
    [SerializeField] protected GameObject objetivo;

    // Update is called once per frame
    void Update()
    {
        // Revisamos si el objetivo no se ha destruido
        if (objetivo != null)
        {
            // Mover el disparo hacia el objetivo
            transform.position = Vector2.MoveTowards(transform.position, objetivo.transform.position, velocidad * Time.deltaTime);
        }
        // Si el objetivo se ha destruido
        else
        {
            // Destruir el disparo
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Si el disparo colisiona con el objetivo
        if (other.gameObject == objetivo)
        {
            // Evitamos los colliders de tipo CircleCollider2D
            if (other is CircleCollider2D)
            {
                return;
            }
            // Obtener el componente del enemigo
            EnemigoBasico enemigo = other.GetComponent<EnemigoBasico>();
            // Si el enemigo existe
            if (enemigo != null)
            {
                // Aplicar el danio al enemigo
                enemigo.RecibirDanio(danio);
            }
            // Destruir el disparo
            Destroy(gameObject);
        }
    }

    // Metodo para actualizar el danio
    public void SetDanio(int nuevoDanio)
    {
        danio = nuevoDanio;
    }

    // Metodo para actualizar el objetivo
    public void SetObjetivo(GameObject nuevoObjetivo)
    {
        objetivo = nuevoObjetivo;
    }
}

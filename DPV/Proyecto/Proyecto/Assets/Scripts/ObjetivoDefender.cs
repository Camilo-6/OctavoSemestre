using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObjetivoDefender : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si colisiona con un enemigo
        if (collision.CompareTag("Enemigo"))
        {
            // Evitamos los colliders de tipo CircleCollider2D
            if (collision is CircleCollider2D)
            {
                return;
            }
            // Destruimos el enemigo
            EnemigoBasico enemigo = collision.GetComponent<EnemigoBasico>();
            if (enemigo != null)
            {
                enemigo.AlcanzarObjetivo();
            }
        }
    }

}

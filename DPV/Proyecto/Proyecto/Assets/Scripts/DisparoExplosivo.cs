using UnityEngine;

public class DisparoExplosivo : DisparoBasico
{
    // Radio de explosion
    [SerializeField] private float radio;

    // Actualizar el onTriggerEnter2D
    protected override void OnTriggerEnter2D(Collider2D other)
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
                // Obtener todos los enemigos en el radio
                Collider2D[] enemigosEnRadio = Physics2D.OverlapCircleAll(transform.position, radio);
                // Aplicar danio a los enemigos en el radio
                foreach (Collider2D enemigoEnRadio in enemigosEnRadio)
                {
                    EnemigoBasico enemigoDentro = enemigoEnRadio.GetComponent<EnemigoBasico>();
                    // Si el enemigo existe y no es el mismo que el objetivo
                    if (enemigoDentro != null && enemigoDentro.gameObject != objetivo)
                    {
                        // Evitar colliders de tipo CircleCollider2D
                        if (enemigoEnRadio is CircleCollider2D)
                        {
                            continue;
                        }
                        // Aplicar el danio al enemigo
                        enemigoDentro.RecibirDanio(danio);
                    }
                }
            }
            // Destruir el disparo
            Destroy(gameObject);
        }
    }

    // Metodo para actualizar el radio
    public void SetRadio(float nuevoRadio)
    {
        radio = nuevoRadio;
    }
}

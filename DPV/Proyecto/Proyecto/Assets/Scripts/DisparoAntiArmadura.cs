using UnityEngine;

public class DisparoAntiArmadura : DisparoBasico
{
    // Cantidad de armadura que el disparo quita al enemigo
    [SerializeField] private int menosArmadura;

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
                // Reducir la armadura del enemigo
                enemigo.ReducirArmadura(menosArmadura);
                // Aplicar el danio al enemigo
                enemigo.RecibirDanio(danio);
            }
            // Destruir el disparo
            Destroy(gameObject);
        }
    }

    // Metodo para actualizar la cantidad de armadura a quitar
    public void SetMenosArmadura(int nuevaCantidad)
    {
        menosArmadura = nuevaCantidad;
    }
}

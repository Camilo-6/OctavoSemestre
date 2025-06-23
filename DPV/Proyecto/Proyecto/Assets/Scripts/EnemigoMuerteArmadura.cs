using UnityEngine;

public class EnemigoEnMuerte : EnemigoBasico
{
    // Rango del efecto de muerte
    [SerializeField] private float rango;
    // Cantidad de armadura que se dara al morir
    [SerializeField] private int armaduraAlMorir;

    // Metodo para realizar acciones al morir
    protected override void AlMorir()
    {
        // Obtenemos todos los enemigos en el rango del efecto de muerte
        Collider2D[] enemigosEnRadio = Physics2D.OverlapCircleAll(transform.position, rango);
        // Aplicar armadura a los enemigos en el radio
        foreach (Collider2D enemigoEnRadio in enemigosEnRadio)
        {
            EnemigoBasico enemigoDentro = enemigoEnRadio.GetComponent<EnemigoBasico>();
            // Si el enemigo existe y no es el enemigo que esta muriendo
            if (enemigoDentro != null && enemigoDentro.gameObject != gameObject)
            {
                // Evitar colliders de tipo CircleCollider2D
                if (enemigoEnRadio is CircleCollider2D)
                {
                    continue;
                }
                // Aplicar la armadura al enemigo
                enemigoDentro.AgregarArmadura(armaduraAlMorir);
            }
        }
    }

    // Mostrar el rango del efecto de muerte en el editor
    private void OnDrawGizmosSelected()
    {
        // Dibujar un circulo en el rango del efecto de muerte
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rango);
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoY : MonoBehaviour
{
    // Rigidbody2D del objeto
    private Rigidbody2D rb2d { get; set; }
    // Configuracion de colisiones
    [field: Header("Configuracion de colisiones"), SerializeField] LayerMask capaPiso { get; set; }
    // Posicion rayo
    [field: SerializeField] private Vector2 posicionRayo { get; set; }
    // Distancia del rayo
    [field: SerializeField] private float distanciaRayo { get; set; } = 0.1f;
    // Rayo pie derecho
    RaycastHit2D rayoPieDerecho { get; set; }
    // Rayo pie izquierdo
    RaycastHit2D rayoPieIzquierdo { get; set; }
    // Fuerza de salto
    [field: Header("Fuerza de salto"), SerializeField] float fuerzaSalto { get; set; }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public bool EnPiso()
    {
        Vector2 InicioDerecho = new(transform.position.x + posicionRayo.x, transform.position.y + posicionRayo.y);
        Vector2 InicioIzquierdo = new(transform.position.x - posicionRayo.x, transform.position.y + posicionRayo.y);
        rayoPieDerecho = Physics2D.Raycast(InicioDerecho, Vector2.down, distanciaRayo, capaPiso);
        rayoPieIzquierdo = Physics2D.Raycast(InicioIzquierdo, Vector2.down, distanciaRayo, capaPiso);
        return rayoPieDerecho || rayoPieIzquierdo;
    }

    public void Saltar()
    {
        rb2d.AddForceY(fuerzaSalto, ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        Vector2 InicioDerecho = new(transform.position.x + posicionRayo.x, transform.position.y + posicionRayo.y);
        Vector2 InicioIzquierdo = new(transform.position.x - posicionRayo.x, transform.position.y + posicionRayo.y);

        Debug.DrawRay(InicioDerecho, Vector3.down * distanciaRayo, Color.red, 0.0f);
        Debug.DrawRay(InicioIzquierdo, Vector3.down * distanciaRayo, Color.red, 0.0f);
    }
}

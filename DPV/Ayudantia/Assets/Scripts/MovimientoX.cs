using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MovimientoX : MonoBehaviour
{
    // Rigidbody
    private Rigidbody2D rb { get; set; }
    // Velocidad maxima
    [field: SerializeField, Header("Velocidad maxima"), Tooltip("Controla velocidad maximo en eje x")] private float velocidadMaxima { get; set; }
    // Velocidad actual
    [SerializeField] private float velocidadActual { get; set; }
    // Direccion en X
    [SerializeField] private float direccionX { get; set; }

    void Awake()
    {
        // Obtener el Rigidbody
        rb = GetComponent<Rigidbody2D>();
        //rb.simulated = false;
        //rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void CambiarDireccion(float direccion)
    {
        direccionX = direccion;
    }

    public void Desacelerar()
    {
        velocidadActual = 0f;
    }

    public void Acelerar()
    {
        velocidadActual = velocidadMaxima;
    }

    public void Mover()
    {
        rb.linearVelocityX = direccionX * velocidadActual;
    }
}
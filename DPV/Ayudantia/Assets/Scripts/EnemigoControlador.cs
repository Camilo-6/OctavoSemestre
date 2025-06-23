using UnityEngine;

public class EnemigoControlador : MonoBehaviour
{
    private MovimientoX movimientoX;

    [Header("Configuración de colisiones")]
    [SerializeField] private LayerMask capaPiso;
    [SerializeField] private LayerMask capaParedes;

    [SerializeField] private Vector2 PosicionRayoIzquierdo;
    [SerializeField] private Vector2 PosicionRayoDerecho;
    [SerializeField] private Vector2 PosicionRayoFrontal;
    [SerializeField] private Vector2 PosicionRayoTrasero;

    [SerializeField] private float DistanciaRayoSuelo;
    [SerializeField] private float DistanciaRayoPared;

    private bool Direccion;
    private bool puedeCambiarDireccion = true;

    void Start()
    {
        movimientoX = GetComponent<MovimientoX>();
        movimientoX.Acelerar();
        Direccion = false;
        movimientoX.CambiarDireccion(-1);
    }

    void Update()
    {
        movimientoX.Mover();

        if ((NoTieneSuelo() || HayParedFrente()) && puedeCambiarDireccion)
        {
            CambiarDireccion();
        }
    }

    private bool NoTieneSuelo()
    {
        Vector2 inicioIzquierdo = (Vector2)transform.position + PosicionRayoIzquierdo;
        Vector2 inicioDerecho = (Vector2)transform.position + PosicionRayoDerecho;

        bool sueloIzquierdo = Physics2D.Raycast(inicioIzquierdo, Vector2.down, DistanciaRayoSuelo, capaPiso);
        bool sueloDerecho = Physics2D.Raycast(inicioDerecho, Vector2.down, DistanciaRayoSuelo, capaPiso);

        return !sueloIzquierdo || !sueloDerecho;
    }

    private bool HayParedFrente()
    {
        Vector2 inicioFrontal = (Vector2)transform.position + PosicionRayoFrontal;
        Vector2 direccionRayo = Direccion ? Vector2.right : Vector2.left;

        return Physics2D.Raycast(inicioFrontal, direccionRayo, DistanciaRayoPared, capaParedes);
    }

    private void CambiarDireccion()
    {
        Direccion = !Direccion;
        movimientoX.CambiarDireccion(Direccion ? 1 : -1);

        (PosicionRayoFrontal, PosicionRayoTrasero) = (PosicionRayoTrasero, PosicionRayoFrontal);

        puedeCambiarDireccion = false;
        Invoke(nameof(ResetearCambioDireccion), 0.2f);
    }

    private void ResetearCambioDireccion()
    {
        puedeCambiarDireccion = true;
    }

    void OnDrawGizmos()
    {
        Vector2 inicioIzquierdo = (Vector2)transform.position + PosicionRayoIzquierdo;
        Vector2 inicioDerecho = (Vector2)transform.position + PosicionRayoDerecho;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(inicioIzquierdo, inicioIzquierdo + Vector2.down * DistanciaRayoSuelo);
        Gizmos.DrawLine(inicioDerecho, inicioDerecho + Vector2.down * DistanciaRayoSuelo);

        Vector2 inicioFrontal = (Vector2)transform.position + PosicionRayoFrontal;
        Vector2 direccionRayo = Direccion ? Vector2.right : Vector2.left;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(inicioFrontal, inicioFrontal + direccionRayo * DistanciaRayoPared);
    }
}

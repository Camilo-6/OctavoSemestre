using UnityEngine;

public class movimiento : MonoBehaviour
{
    // Velocidad
    [SerializeField] private float velocidad = 2.0f;
    // Movimiento vertical
    [SerializeField] private float movimientoVertical;
    // Movimiento horizontal
    [SerializeField] private float movimientoHorizontal;
    // Rigidbody
    [SerializeField] private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movimientoHorizontal = Input.GetAxis("Horizontal");
        movimientoVertical = Input.GetAxis("Vertical");
        
        // Empujar el objeto la version vista en clase
        Vector3 movimiento = new Vector3(movimientoHorizontal, 0.0f, movimientoVertical);
        rb.AddForce(movimiento * velocidad);
    }        
}

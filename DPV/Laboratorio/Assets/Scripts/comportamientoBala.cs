using UnityEngine;

public class comportamientoBala : MonoBehaviour
{
    // Velocidad
    [SerializeField] private float velocidad;
    // Rigidbody
    [SerializeField] private Rigidbody rb;
    // Tiempo de vida
    [SerializeField] private float tiempoVida;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.AddForce(velocidad * transform.forward);
        Destroy(this.gameObject, tiempoVida);
    }

    // Quitamos el update ya que no lo necesitamos

    // Cuando colisiona con otro objeto
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemigo destruible") || other.gameObject.CompareTag("objeto destruible"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("escenario"))
        {
            Destroy(this.gameObject);
        }
    }
}

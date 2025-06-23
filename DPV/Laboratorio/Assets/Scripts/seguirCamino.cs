using UnityEngine;

public class seguirCamino : MonoBehaviour
{
    // Puntos del camino
    [SerializeField] private Transform[] puntos;
    // Velocidad
    [SerializeField] private float velocidad = 4.0f;
    // Punto actual
    private int puntoActual = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rotar el objeto hacia el primer punto
        transform.LookAt(puntos[puntoActual].position);
    }

    // Update is called once per frame
    void Update()
    {
        // Mover el objeto hacia el siguiente punto
        transform.position = Vector3.MoveTowards(transform.position, puntos[puntoActual].position, velocidad * Time.deltaTime);
        // Si el objeto llega al punto
        if (transform.position == puntos[puntoActual].position)
        {
            // Cambiar al siguiente punto
            puntoActual++;
            // Si no existe el siguiente punto, reiniciar el punto actual
            if (puntoActual >= puntos.Length)
            {
                puntoActual = 0;
            }
            // Rotar el objeto hacia el siguiente punto
            transform.LookAt(puntos[puntoActual].position);
        }
    }
}

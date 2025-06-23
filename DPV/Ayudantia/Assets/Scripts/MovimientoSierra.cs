using UnityEngine;
using System.Collections.Generic;

public class MovimientoSierra : MonoBehaviour
{
    // Sierra
    [SerializeField] private GameObject sierra;
    // Objeto a crear
    [SerializeField] private GameObject crear;
    // Posicion inicial
    private Vector2 posicionInicial;
    // Posicion final
    [SerializeField] private Vector2 posicionFinal;
    // Distancia de movimiento
    private float distanciaMovimiento = 0;
    // Cantidad objetos a crear
    [SerializeField] private int cantidadObjetos;
    // Lista de posiciones
    private List<float> posiciones = new List<float>();
    // Ultima posicion
    private float ultimaPosicion = float.MaxValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicionInicial = sierra.transform.position;
        // Si la posicion inicial y final estan definidas
        if (posicionInicial != null && posicionFinal != null)
        {
            // Calculamos la distancia de movimiento
            distanciaMovimiento = posicionFinal.x - posicionInicial.x;
            // Si la distancia de movimiento es diferente de 0 y la cantidad de objetos es mayor a 0
            if (distanciaMovimiento != 0 && cantidadObjetos > 0)
            {
                cantidadObjetos -= 1;
                // Dividimos la distancia de movimiento entre la cantidad de objetos
                float distancia = distanciaMovimiento / cantidadObjetos;
                // Agregamos las posiciones donde crearemos objetos
                float pos_actual = posicionInicial.x;
                for (int i = 0; i < cantidadObjetos; i++)
                {
                    posiciones.Add(pos_actual);
                    pos_actual += distancia;
                }
                posiciones.Add(posicionFinal.x);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Si la distancia de movimiento es diferente de 0
        if (distanciaMovimiento != 0)
        {
            // Vamos a mover el objeto desde la posicion inicial a la final y luego de la final a la inicial
            float movimiento = Mathf.PingPong(Time.time, distanciaMovimiento) / distanciaMovimiento;
            sierra.transform.position = Vector2.Lerp(posicionInicial, posicionFinal, movimiento);
            // Revisar si estamos en una posicion para crear un objeto
            if (posiciones.Count > 0)
            {
                float xActual = sierra.transform.position.x;
                float xObjetivo = posiciones[0];
                // Si el objeto esta en la posicion objetivo y no se ha creado un objeto en esa posicion
                if (Mathf.Abs(xActual - xObjetivo) < 0.01f && xObjetivo != ultimaPosicion)
                {
                    // Creamos el objeto
                    Vector3 posicionCreacion = new Vector3(sierra.transform.position.x, sierra.transform.position.y, sierra.transform.position.z - 1);
                    GameObject nuevoObjeto = Instantiate(crear, posicionCreacion, Quaternion.identity);
                    // Asignamos el padre
                    nuevoObjeto.transform.SetParent(transform);
                    // Eliminamos la posicion de la lista
                    posiciones.RemoveAt(0);
                    // Actualizamos la ultima posicion
                    ultimaPosicion = xObjetivo;
                }
            }
        }
    }
}

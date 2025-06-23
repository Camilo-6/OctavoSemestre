using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPlataforma : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 posicionInicial;    // Posicion donde comienza la plataforma
    [Header("Movimiento"), SerializeField, Tooltip("Duracion del movimiento en una direccion")]
    float tiempo = 1;
    [SerializeField, Tooltip("Posicion donde se va a mover la plataforma")]
    Vector2 posicionFinal;
    Vector2 vectorDireccion;   //  La direccion donde se mueve la plataforma
    bool direccion; //  Valor que nos dice a que direccion debe moverse el objeto
    Coroutine coroutine;
    // Objeto plataforma
    [SerializeField] private GameObject plataforma;
    // Objeto a crear
    [SerializeField] private GameObject crear;
    // Cantidad de objetos a crear
    [SerializeField] private int cantidadObjetos;
    // Lista de posiciones
    private List<Vector2> posiciones = new List<Vector2>();

    private void Start()
    {
        //  Movimiento
        rb = plataforma.GetComponent<Rigidbody2D>();
        posicionInicial = plataforma.transform.position;
        vectorDireccion = posicionFinal - posicionInicial;

        // Si la posicion inicial y final estan definidas
        if (posicionInicial != null && posicionFinal != null)
        {
            // Calculamos la distancia de movimiento en x
            float distanciaMovimientoX = posicionFinal.x - posicionInicial.x;
            // Calculamos la distancia de movimiento en y
            float distanciaMovimientoY = posicionFinal.y - posicionInicial.y;
            // Si la cantidad de objetos es mayor a 0
            if (cantidadObjetos > 0)
            {
                cantidadObjetos -= 1;
                // Dividimos la distancia de movimiento entre la cantidad de objetos
                float distanciaX = distanciaMovimientoX / cantidadObjetos;
                float distanciaY = distanciaMovimientoY / cantidadObjetos;
                // Agregamos las posiciones donde crearemos objetos
                float pos_actual_x = posicionInicial.x;
                float pos_actual_y = posicionInicial.y;
                for (int i = 0; i < cantidadObjetos; i++)
                {
                    posiciones.Add(new Vector2(pos_actual_x, pos_actual_y));
                    pos_actual_x += distanciaX;
                    pos_actual_y += distanciaY;
                }
                posiciones.Add(posicionFinal);
                cantidadObjetos += 1;
            }
        }

        //  Activar comportamiento
        Activar();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direccion ? -vectorDireccion / tiempo : vectorDireccion / tiempo;
        // Revisar si estamos en una posicion para crear un objeto
        if (posiciones.Count > 0)
        {
            float xActual = plataforma.transform.position.x;
            float xSiguiente = posiciones[0].x;
            float yActual = plataforma.transform.position.y;
            float ySiguiente = posiciones[0].y;
            // Si la plataforma esta en una posicion para crear un objeto
            if (Math.Abs(xActual - xSiguiente) < 0.1 && Math.Abs(yActual - ySiguiente) < 0.1)
            {
                // Crear objeto
                Vector3 posicion = new Vector3(xSiguiente, ySiguiente, 2);
                GameObject nuevo = Instantiate(crear, posicion, Quaternion.identity);
                // Ajustamos la escala del objeto a la misma escala que este objeto
                nuevo.transform.localScale = transform.localScale;
                // Asignamos el padre
                nuevo.transform.SetParent(transform);
                // Eliminamos la posicion de la lista
                posiciones.RemoveAt(0);
            }
        }
    }

    IEnumerator CambiarDireccion()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempo);
            direccion = !direccion;
        }
    }

    private void Activar()
    {
        coroutine ??= StartCoroutine(nameof(CambiarDireccion));
        Animator animator = plataforma.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("apagado", false);
        }
    }

    private void Desactivar()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        Animator animator = plataforma.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("apagado", true);
        }
    }
}

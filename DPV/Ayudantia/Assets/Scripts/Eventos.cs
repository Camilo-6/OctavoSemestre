using System;
using UnityEngine;
using UnityEngine.Events;

public class Eventos : MonoBehaviour
{
    // Singleton?
    public static Eventos instancia;

    // Evento de Unity
    public UnityEvent evento;
    // Accion, se modifica en codigo
    public Action accion;

    // Inicializacion
    private void Awake()
    {
        // Singleton?
        if (instancia == null)
        {
            instancia = this;
        }
    }

    public void Metodo1()
    {
        evento?.Invoke(); // El operador ? es para evitar errores si el evento es null
    }

    public void Metodo2()
    {
        accion?.Invoke();
    }

    // Subscribir evento
    public void SubscribirEvento(Action accion)
    {
        this.accion += accion;
    }

    // Desubscribir evento
    public void DesubscribirEvento(Action accion)
    {
        this.accion -= accion;
    }
}

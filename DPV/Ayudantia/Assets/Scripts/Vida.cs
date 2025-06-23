using System;
using UnityEngine;

public class Vida : MonoBehaviour
{
    // Vida maxima
    public float vidaMaxima;
    // Vida actual
    public float vidaActual;
    // Accciiones
    public static Action<float> VidaAction;
    // Accion de muerte
    public static Action MuerteAction;

    // Start is called before the first frame update
    void Start()
    {
        vidaActual = vidaMaxima;
    }

    // Perder vida
    public void PerderVida(float cantidad)
    {
        if (vidaActual - cantidad > 0)
        {
            vidaActual -= cantidad;
            VidaAction?.Invoke(vidaActual / vidaMaxima);
        }
        else
        {
            vidaActual = 0;
            VidaAction?.Invoke(0);
            MuerteAction?.Invoke();
            // Accion de muerte
        }
    }

}

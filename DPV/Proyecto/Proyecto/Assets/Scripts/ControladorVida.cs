using UnityEngine;

public class ControladorVida : MonoBehaviour
{
    // Barra de vida
    public Transform barra;
    // Escala inicial en X
    private float escalaInicialX = 0.6713338f;

    // Metodo para actualizar la barra de vida
    public void ActualizarVida(int vidaActual, int vidaMaxima)
    {
        // Calcular el nuevo porcentaje de vida
        float porcentaje = (float)vidaActual / vidaMaxima;
        porcentaje = Mathf.Clamp01(porcentaje);
        // Cambia la escala de la barra de vida en X
        Vector3 nuevaEscala = new Vector3(escalaInicialX * porcentaje, barra.localScale.y, barra.localScale.z);
        barra.localScale = nuevaEscala;
    }
}

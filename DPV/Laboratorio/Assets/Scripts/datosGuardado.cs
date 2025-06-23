using UnityEngine;

[System.Serializable]

public class datosGuardado
{
    // Posicion del jugador
    public Vector3 posicionJugador;
    // Rotacion del jugador
    public Quaternion rotacionJugador;

    // Constructor
    public datosGuardado(Vector3 posicion, Quaternion rotacion)
    {
        posicionJugador = posicion;
        rotacionJugador = rotacion;
    }

}

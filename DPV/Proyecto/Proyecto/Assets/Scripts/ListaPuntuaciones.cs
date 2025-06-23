using System.Collections.Generic;

// Clase para almacenar las puntaciones de los jugadores
[System.Serializable]
public class ListaPuntaciones
{
    // Lista de puntaciones
    public List<Puntacion> puntaciones;

    // Constructor que recibe una lista de puntaciones
    public ListaPuntaciones(List<Puntacion> puntaciones)
    {
        this.puntaciones = puntaciones;
    }
}

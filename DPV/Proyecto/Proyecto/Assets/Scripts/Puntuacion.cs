[System.Serializable]

// Clase para almacenar la puntuacion de un jugador
public class Puntacion
{
    // Nombre del jugador
    public string nombreJugador;
    // Puntuacion del jugador
    public int puntuacion;

    // Constructor que recibe el nombre del jugador y la puntuacion
    public Puntacion(string nombreJugador, int puntuacion)
    {
        this.nombreJugador = nombreJugador;
        this.puntuacion = puntuacion;
    }
}

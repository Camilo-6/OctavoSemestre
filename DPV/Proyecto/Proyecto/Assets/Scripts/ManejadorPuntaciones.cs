using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ManajadorPuntaciones : MonoBehaviour
{
    // Metodo para obtener las puntaciones del nivel
    public List<Puntacion> ObtenerPuntaciones(string archivo)
    {
        // Revisamos si el archivo existe
        if (File.Exists(archivo))
        {
            try
            {
                // Leemos el archivo
                string cadenajson = File.ReadAllText(archivo);
                // Deserializamos el archivo
                ListaPuntaciones datos = JsonUtility.FromJson<ListaPuntaciones>(cadenajson);
                // Revisamos si hay puntaciones
                if (datos == null || datos.puntaciones == null)
                {
                    // Si no hay puntaciones, regresamos y guardamos una lista vacia
                    List<Puntacion> vacia = new List<Puntacion>();
                    ListaPuntaciones datosVacia = new ListaPuntaciones(vacia);
                    // Serializamos el objeto de datos
                    string cadenajsonVacia = JsonUtility.ToJson(datosVacia, true);
                    // Guardamos el archivo
                    File.WriteAllText(archivo, cadenajsonVacia);
                    // Retornamos una lista vacia
                    return new List<Puntacion>();
                }
                // Obtenemos las puntaciones
                List<Puntacion> puntaciones = datos.puntaciones;
                // Ordenamos las puntaciones
                return puntaciones;
            }
            catch
            {
                // Si hay un error, regresamos y guardamos una lista vacia
                List<Puntacion> vacia = new List<Puntacion>();
                ListaPuntaciones datos = new ListaPuntaciones(vacia);
                // Serializamos el objeto de datos
                string cadenajson = JsonUtility.ToJson(datos, true);
                // Guardamos el archivo
                File.WriteAllText(archivo, cadenajson);
                // Retornamos una lista vacia
                return new List<Puntacion>();
            }
        }
        // Si el archivo no existe
        else
        {
            // Creamos una lista vacia
            List<Puntacion> vacia = new List<Puntacion>();
            // Creamos el objeto de datos
            ListaPuntaciones datos = new ListaPuntaciones(vacia);
            // Serializamos el objeto de datos
            string cadenajson = JsonUtility.ToJson(datos, true);
            // Guardamos el archivo
            File.WriteAllText(archivo, cadenajson);
            // Retornamos una lista vacia
            return new List<Puntacion>();
        }
    }

    // Metodo para guardar una nueva puntacion
    public void GuardarPuntacion(string nombreJugador, int puntuacion, string archivo)
    {
        // Obtenemos las puntaciones
        List<Puntacion> puntaciones = ObtenerPuntaciones(archivo);
        // Agregamos la nueva puntacion
        puntaciones.Add(new Puntacion(nombreJugador, puntuacion));
        // Revisamos si hay mas de 5 puntaciones
        if (puntaciones.Count > 5)
        {
            // Ordenamos las puntaciones de menor a mayor
            puntaciones.Sort((x, y) => y.puntuacion.CompareTo(x.puntuacion));
            // Eliminamos la menor
            puntaciones.RemoveAt(puntaciones.Count - 1);
        }
        // Creamos el objeto de datos
        ListaPuntaciones datos = new ListaPuntaciones(puntaciones);
        // Serializamos el objeto de datos
        string cadenajson = JsonUtility.ToJson(datos, true);
        // Guardamos el archivo
        File.WriteAllText(archivo, cadenajson);
    }

    // Metodo para revisar si la puntacion actual entra en el top 5
    public bool RevisarPuntacion(int puntuacion, string archivo)
    {
        // Obtenemos las puntaciones
        List<Puntacion> puntaciones = ObtenerPuntaciones(archivo);
        // Revisamos si hay menos de 5 puntaciones
        if (puntaciones.Count < 5)
        {
            // Si hay menos de 5, la puntacion entra
            return true;
        }
        // Ordenamos las puntaciones de menor a mayor
        puntaciones.Sort((x, y) => y.puntuacion.CompareTo(x.puntuacion));
        // Revisamos si la puntacion es mayor a la menor
        if (puntuacion > puntaciones[4].puntuacion)
        {
            // Si es mayor, la puntacion entra
            return true;
        }
        // Si no, la puntacion no entra
        return false;
    }
}
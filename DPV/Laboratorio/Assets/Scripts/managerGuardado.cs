using UnityEngine;
using System.IO;

public class managerGuardado : MonoBehaviour
{
    // Jugador
    private GameObject jugador;
    // Nombre del archivo de guardado
    public string nombreArchivo;
    // Clase de datos de guardado
    private datosGuardado dg = new datosGuardado(Vector3.zero, Quaternion.identity);

    private void Awake()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        nombreArchivo = Application.dataPath + "/guardado.json";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            guardarDatos();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            cargarDatos();
        }
    }

    // Guardar datos
    public void guardarDatos()
    {
        datosGuardado nuevosDatos = new datosGuardado(jugador.transform.position, jugador.transform.rotation);
        string cadenajson = JsonUtility.ToJson(nuevosDatos, true);
        File.WriteAllText(nombreArchivo, cadenajson);
    }

    // Cargar datos
    public void cargarDatos()
    {
        if (File.Exists(nombreArchivo))
        {
            string cadenajson = File.ReadAllText(nombreArchivo);
            dg = JsonUtility.FromJson<datosGuardado>(cadenajson);
            jugador.transform.SetPositionAndRotation(dg.posicionJugador, dg.rotacionJugador); // no sirve xd
        }
    }
}

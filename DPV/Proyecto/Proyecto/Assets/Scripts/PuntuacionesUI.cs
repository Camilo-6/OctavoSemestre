using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class PuntuacionesUI : MonoBehaviour
{
    // Manajedor de puntaciones
    [SerializeField] private ManajadorPuntaciones manajadorPuntaciones;
    // Texto del titulo
    [SerializeField] private TMP_Text titulo;
    // Texto no hay puntaciones
    [SerializeField] private TMP_Text textoNoHayPuntaciones;
    // Texto titulo nombre jugador
    [SerializeField] private TMP_Text tituloNombreJugador;
    // Texto titulo puntacion
    [SerializeField] private TMP_Text tituloPuntacion;
    // Texto nombre1
    [SerializeField] private TMP_Text nombre1;
    // Texto puntacion1
    [SerializeField] private TMP_Text puntacion1;
    // Texto nombre2
    [SerializeField] private TMP_Text nombre2;
    // Texto puntacion2
    [SerializeField] private TMP_Text puntacion2;
    // Texto nombre3
    [SerializeField] private TMP_Text nombre3;
    // Texto puntacion3
    [SerializeField] private TMP_Text puntacion3;
    // Texto nombre4
    [SerializeField] private TMP_Text nombre4;
    // Texto puntacion4
    [SerializeField] private TMP_Text puntacion4;
    // Texto nombre5
    [SerializeField] private TMP_Text nombre5;
    // Texto puntacion5
    [SerializeField] private TMP_Text puntacion5;
    // Nombre del archivo de puntaciones
    private string archivo;

    void Start()
    {
        // Obtenemos el nivel actual
        string escenaNivel = PlayerPrefs.GetString("Escena Nivel", "Nivel1");
        // Obtenemos el nombre del archivo
        archivo = Path.Combine(Application.persistentDataPath, "puntuaciones_" + escenaNivel + ".json");
        // Obtenemos el nivel del cual seran las puntaciones
        string nivel;
        switch (escenaNivel)
        {
            case "Nivel1":
                nivel = "1";
                break;
            case "Nivel2":
                nivel = "2";
                break;
            case "Nivel3":
                nivel = "3";
                break;
            default:
                nivel = "1";
                break;
        }
        // Actualizamos el titulo
        titulo.text = "Mejores puntaciones del Nivel " + nivel;
        // Obtenemos las puntaciones
        List<Puntacion> puntaciones = manajadorPuntaciones.ObtenerPuntaciones(archivo);
        // Ordenamos las puntaciones de mayor a menor
        puntaciones.Sort((x, y) => y.puntuacion.CompareTo(x.puntuacion));
        // Revisamos si no hay puntaciones
        if (puntaciones.Count == 0)
        {
            // Mostramos el texto de no hay puntaciones
            textoNoHayPuntaciones.gameObject.SetActive(true);
        }
        else
        {
            // Mostramos los titulos de nombre y puntacion
            tituloNombreJugador.gameObject.SetActive(true);
            tituloPuntacion.gameObject.SetActive(true);
            // Mostramos las primeras 5 puntaciones
            for (int i = 0; i < puntaciones.Count && i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        nombre1.text = puntaciones[i].nombreJugador;
                        puntacion1.text = puntaciones[i].puntuacion.ToString();
                        nombre1.gameObject.SetActive(true);
                        puntacion1.gameObject.SetActive(true);
                        break;
                    case 1:
                        nombre2.text = puntaciones[i].nombreJugador;
                        puntacion2.text = puntaciones[i].puntuacion.ToString();
                        nombre2.gameObject.SetActive(true);
                        puntacion2.gameObject.SetActive(true);
                        break;
                    case 2:
                        nombre3.text = puntaciones[i].nombreJugador;
                        puntacion3.text = puntaciones[i].puntuacion.ToString();
                        nombre3.gameObject.SetActive(true);
                        puntacion3.gameObject.SetActive(true);
                        break;
                    case 3:
                        nombre4.text = puntaciones[i].nombreJugador;
                        puntacion4.text = puntaciones[i].puntuacion.ToString();
                        nombre4.gameObject.SetActive(true);
                        puntacion4.gameObject.SetActive(true);
                        break;
                    case 4:
                        nombre5.text = puntaciones[i].nombreJugador;
                        puntacion5.text = puntaciones[i].puntuacion.ToString();
                        nombre5.gameObject.SetActive(true);
                        puntacion5.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    // Metodo para volver al menu principal
    public void VolverAMenuPrincipal()
    {
        // Volvemos al menu principal
        SceneManager.LoadScene("MenuPrincipal");
    }
}
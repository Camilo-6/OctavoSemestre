using UnityEngine;
using TMPro;

public class ControladorGameStatsUI : MonoBehaviour
{
    // Texto de vida actual
    [SerializeField] private TMP_Text vidaActualTexto;
    // Texto de vida total
    [SerializeField] private TMP_Text vidaTotalTexto;
    // Texto de ronda actual
    [SerializeField] private TMP_Text rondaActualTexto;
    // Texto de ronda total
    [SerializeField] private TMP_Text rondaTotalTexto;
    // Controlador de juego
    [SerializeField] private ControladorJuego controladorJuego;
    // Vidas actuales
    private int vidasActuales;
    // Ronda actual
    private int rondaActual;

    // Metodo para actualizar el texto de vidas actuales
    void ActualizarTextoVidasActuales()
    {
        vidasActuales = controladorJuego.getVidasActuales();
        vidaActualTexto.text = vidasActuales.ToString();
    }

    // Metodo para actualizar el texto de ronda actual
    void ActualizarTextoRondaActual()
    {
        rondaActual = controladorJuego.getRondaActual();
        rondaActualTexto.text = rondaActual.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Actualizar los textos de vidas y rondas
        ActualizarTextoVidasActuales();
        vidaTotalTexto.text = controladorJuego.getVidasTotales().ToString();
        ActualizarTextoRondaActual();
        rondaTotalTexto.text = controladorJuego.getTotalRondas().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Si hay cambios en las vidas o rondas, actualizar los textos
        if (controladorJuego.getVidasActuales() != vidasActuales)
        {
            ActualizarTextoVidasActuales();
        }
        if (controladorJuego.getRondaActual() != rondaActual)
        {
            ActualizarTextoRondaActual();
        }
    }
}

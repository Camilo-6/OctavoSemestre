using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class prefs : MonoBehaviour
{
    // Texto de los puntos
    public TMP_Text puntosTexto;

    /*
    void Awake()
    {
        puntosTexto.text = PlayerPrefs.GetInt("puntos", 0).ToString();
    }
    */

    public void SaveData()
    {
        // Guardar los puntos en PlayerPrefs
        PlayerPrefs.SetInt("puntos", int.Parse(puntosTexto.text));
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        // Cargar los puntos de PlayerPrefs
        if (PlayerPrefs.HasKey("puntos"))
        {
            int puntos = PlayerPrefs.GetInt("puntos");
            puntosTexto.text = puntos.ToString();
        }
        else
        {
            puntosTexto.text = "0"; // Valor por defecto si no hay datos guardados
        }
    }

    public void ResetData()
    {
        // Reiniciar los puntos a 0
        PlayerPrefs.SetInt("puntos", 0);
        // PlayerPrefs.DeleteKey("puntos"); // Eliminar la clave de PlayerPrefs, es otra forma
        PlayerPrefs.Save();
        puntosTexto.text = "0"; // Actualizar el texto a 0
    }
}
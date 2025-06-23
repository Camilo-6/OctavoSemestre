using UnityEngine;

public class ObtenerDimensionesCamara : MonoBehaviour
{
    void Start()
    {
        Camera camaraPrincipal = Camera.main;

        if (camaraPrincipal != null)
        {
            float ancho = camaraPrincipal.pixelWidth;
            float alto = camaraPrincipal.pixelHeight;

            Debug.Log("Ancho de la camara: " + ancho);
            Debug.Log("Alto de la camara: " + alto);
        }
        else
        {
            Debug.LogError("No se encontro la camara principal");
        }
    }
}

// Importante
// Al momento de realizar el build, se debe ajustar el tamanio de la ventana para que sea igual al de las escenas
// Para hacer esto, se debe ir a File > Build Profiles > Player Settings > Resolution and Presentation
// En Fullscreen Mode, seleccionar Windowed
// En Default Screen Width y Default Screen Height, poner 1233 y 569, respectivamente
// Por ultimo, quitar la marca de verificacion de "Resizable Window" para que no se pueda cambiar el tamanio de la ventana
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Configuracion de Imagenes")]
    [SerializeField] private Sprite[] imagenesTutorial; // Todas las imágenes del tutorial
    [SerializeField] private Image imagenDisplay;       // Componente Image para mostrar las imágenes

    [Header("Configuracion de Botones")]
    [SerializeField] private Button botonSiguiente;    // Botón para avanzar
    [SerializeField] private Button botonAnterior;     // Botón para retroceder
    [SerializeField] private Button botonMenuPrincipal; // Botón para regresar al menú

    private int indiceImagenActual = 0;

    private void Start()
    {
        // Inicializar la primera imagen
        ActualizarImagen();

        // Configurar listeners de botones
        botonSiguiente.onClick.AddListener(SiguienteImagen);
        botonMenuPrincipal.onClick.AddListener(RegresarAMenu);

        // Si hay botón anterior, configurarlo
        if (botonAnterior != null)
        {
            botonAnterior.onClick.AddListener(ImagenAnterior);
        }
    }

    private void ActualizarImagen()
    {
        // Mostrar la imagen actual
        imagenDisplay.sprite = imagenesTutorial[indiceImagenActual];

        // Actualizar estado de los botones
        botonSiguiente.gameObject.SetActive(indiceImagenActual < imagenesTutorial.Length - 1);
        botonMenuPrincipal.gameObject.SetActive(indiceImagenActual == imagenesTutorial.Length - 1);

        if (botonAnterior != null)
        {
            botonAnterior.gameObject.SetActive(indiceImagenActual > 0);
        }
    }

    //Siguiente Imagen en la lista
    private void SiguienteImagen()
    {
        if (indiceImagenActual < imagenesTutorial.Length - 1)
        {
            indiceImagenActual++;
            ActualizarImagen();
        }
    }

    //Imagen anterior en la lista
    private void ImagenAnterior()
    {
        if (indiceImagenActual > 0)
        {
            indiceImagenActual--;
            ActualizarImagen();
        }
    }

    //Regreso Menú Principal
    private void RegresarAMenu()
    {
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UIVida : MonoBehaviour
{
    // Imagen de vida (puede ser un texto, una imagen, etc)
    public Image vidaImagen;

    // Start() y OnDestry() sirven para cuando creamos y destruimos el objeto
    // OnEnable() y OnDisable() sirven para cuando activamos y desactivamos el objeto

    // Start is called before the first frame update
    void Start()
    {
        Vida.VidaAction += ActualizarVida;
    }

    void OnDestroy()
    {
        Vida.VidaAction -= ActualizarVida;
    }

    void OnEnable()
    {
        Vida.VidaAction += ActualizarVida;
    }

    void OnDisable()
    {
        Vida.VidaAction -= ActualizarVida;
    }

    public void ActualizarVida(float vida)
    {
        // Actualizar vida
        vidaImagen.fillAmount = vida;
    }
}

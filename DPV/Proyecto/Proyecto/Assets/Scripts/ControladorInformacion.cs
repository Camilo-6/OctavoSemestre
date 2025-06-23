using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorInformacion : MonoBehaviour
{
    // Imagen1
    [SerializeField] private Image imagen1;
    // Descipcion1
    [SerializeField] private TMP_Text descripcion1;
    // Precio1_1
    [SerializeField] private TMP_Text precio1_1;
    // Recurso1_1
    [SerializeField] private Image recurso1_1;
    // Precio1_2
    [SerializeField] private TMP_Text precio1_2;
    // Recurso1_2
    [SerializeField] private Image recurso1_2;
    // Precio1_3
    [SerializeField] private TMP_Text precio1_3;
    // Recurso1_3
    [SerializeField] private Image recurso1_3;
    // Imagen2
    [SerializeField] private Image imagen2;
    // Descipcion2
    [SerializeField] private TMP_Text descripcion2;
    // Precio2_1
    [SerializeField] private TMP_Text precio2_1;
    // Recurso2_1
    [SerializeField] private Image recurso2_1;
    // Precio2_2
    [SerializeField] private TMP_Text precio2_2;
    // Recurso2_2
    [SerializeField] private Image recurso2_2;
    // Precio2_3
    [SerializeField] private TMP_Text precio2_3;
    // Recurso2_3
    [SerializeField] private Image recurso2_3;
    // Imagen3
    [SerializeField] private Image imagen3;
    // Descipcion3
    [SerializeField] private TMP_Text descripcion3;
    // Precio3_1
    [SerializeField] private TMP_Text precio3_1;
    // Recurso3_1
    [SerializeField] private Image recurso3_1;
    // Precio3_2
    [SerializeField] private TMP_Text precio3_2;
    // Recurso3_2
    [SerializeField] private Image recurso3_2;
    // Precio3_3
    [SerializeField] private TMP_Text precio3_3;
    // Recurso3_3
    [SerializeField] private Image recurso3_3;
    // Imagen4
    [SerializeField] private Image imagen4;
    // Descipcion4
    [SerializeField] private TMP_Text descripcion4;
    // Precio4_1
    [SerializeField] private TMP_Text precio4_1;
    // Recurso4_1
    [SerializeField] private Image recurso4_1;
    // Precio4_2
    [SerializeField] private TMP_Text precio4_2;
    // Recurso4_2
    [SerializeField] private Image recurso4_2;
    // Precio4_3
    [SerializeField] private TMP_Text precio4_3;
    // Recurso4_3
    [SerializeField] private Image recurso4_3;
    [SerializeField] private Sprite imagenMejora;
    [SerializeField] private Sprite imagenVender;

    // Start is called before the first frame update
    void Start()
    {
        // Desactivamos todas las opciones
        DesactivarOpciones();
    }

    // Metodo para mostrar la informacion de mejora
    public void MostrarInformacionMejora(string descripcion, int precio1, int precio2, int precio3, Color color)
    {
        // Activamos la opcion 1
        imagen1.gameObject.SetActive(true);
        descripcion1.gameObject.SetActive(true);
        precio1_1.gameObject.SetActive(true);
        recurso1_1.gameObject.SetActive(true);
        precio1_2.gameObject.SetActive(true);
        recurso1_2.gameObject.SetActive(true);
        precio1_3.gameObject.SetActive(true);
        recurso1_3.gameObject.SetActive(true);
        // Asignamos la descripcion y los precios
        descripcion1.text = descripcion;
        precio1_1.text = precio1.ToString();
        precio1_2.text = precio2.ToString();
        precio1_3.text = precio3.ToString();
        // Asignamos el color a la imagen
        imagen1.sprite = imagenMejora;
        imagen1.color = color;
    }

    // Metodo para mostrar la informacion de venta
    public void MostrarInformacionVenta(int precio1, int precio2, int precio3, Color color)
    {
        // Activamos la opcion 2
        imagen2.gameObject.SetActive(true);
        descripcion2.gameObject.SetActive(true);
        precio2_1.gameObject.SetActive(true);
        recurso2_1.gameObject.SetActive(true);
        precio2_2.gameObject.SetActive(true);
        recurso2_2.gameObject.SetActive(true);
        precio2_3.gameObject.SetActive(true);
        recurso2_3.gameObject.SetActive(true);
        // Asignamos la descripcion y los precios
        descripcion2.text = "Vender por recursos";
        precio2_1.text = precio1.ToString();
        precio2_2.text = precio2.ToString();
        precio2_3.text = precio3.ToString();
        // Asignamos el color a la imagen
        imagen2.sprite = imagenVender;
        imagen2.color = color;
    }

    // Metodo para mostrar la informacion de construccion de la opcion 1
    public void MostrarInformacionConstruccion1(string descripcion, int precio1, int precio2, int precio3, Color color, Sprite imagen)
    {
        // Activamos la opcion 1
        imagen1.sprite = imagen;
        imagen1.gameObject.SetActive(true);
        descripcion1.gameObject.SetActive(true);
        precio1_1.gameObject.SetActive(true);
        recurso1_1.gameObject.SetActive(true);
        precio1_2.gameObject.SetActive(true);
        recurso1_2.gameObject.SetActive(true);
        precio1_3.gameObject.SetActive(true);
        recurso1_3.gameObject.SetActive(true);
        // Asignamos la descripcion y los precios
        descripcion1.text = descripcion;
        precio1_1.text = precio1.ToString();
        precio1_2.text = precio2.ToString();
        precio1_3.text = precio3.ToString();
        // Asignamos el color a la imagen
        imagen1.color = color;
    }

    // Metodo para mostrar la informacion de construccion de la opcion 2
    public void MostrarInformacionConstruccion2(string descripcion, int precio1, int precio2, int precio3, Color color, Sprite imagen)
    {
        // Activamos la opcion 2
        imagen2.sprite = imagen;
        imagen2.gameObject.SetActive(true);
        descripcion2.gameObject.SetActive(true);
        precio2_1.gameObject.SetActive(true);
        recurso2_1.gameObject.SetActive(true);
        precio2_2.gameObject.SetActive(true);
        recurso2_2.gameObject.SetActive(true);
        precio2_3.gameObject.SetActive(true);
        recurso2_3.gameObject.SetActive(true);
        // Asignamos la descripcion y los precios
        descripcion2.text = descripcion;
        precio2_1.text = precio1.ToString();
        precio2_2.text = precio2.ToString();
        precio2_3.text = precio3.ToString();
        // Asignamos el color a la imagen
        imagen2.color = color;
    }

    // Metodo para mostrar la informacion de construccion de la opcion 3
    public void MostrarInformacionConstruccion3(string descripcion, int precio1, int precio2, int precio3, Color color, Sprite imagen)
    {
        // Activamos la opcion 3
        imagen3.sprite = imagen;
        imagen3.gameObject.SetActive(true);
        descripcion3.gameObject.SetActive(true);
        precio3_1.gameObject.SetActive(true);
        recurso3_1.gameObject.SetActive(true);
        precio3_2.gameObject.SetActive(true);
        recurso3_2.gameObject.SetActive(true);
        precio3_3.gameObject.SetActive(true);
        recurso3_3.gameObject.SetActive(true);
        // Asignamos la descripcion y los precios
        descripcion3.text = descripcion;
        precio3_1.text = precio1.ToString();
        precio3_2.text = precio2.ToString();
        precio3_3.text = precio3.ToString();
        // Asignamos el color a la imagen
        imagen3.color = color;
    }

    // Metodo para mostrar la informacion de construccion de la opcion 4
    public void MostrarInformacionConstruccion4(string descripcion, int precio1, int precio2, int precio3, Color color, Sprite imagen)
    {
        // Activamos la opcion 4
        imagen4.sprite = imagen;
        imagen4.gameObject.SetActive(true);
        descripcion4.gameObject.SetActive(true);
        precio4_1.gameObject.SetActive(true);
        recurso4_1.gameObject.SetActive(true);
        precio4_2.gameObject.SetActive(true);
        recurso4_2.gameObject.SetActive(true);
        precio4_3.gameObject.SetActive(true);
        recurso4_3.gameObject.SetActive(true);
        // Asignamos la descripcion y los precios
        descripcion4.text = descripcion;
        precio4_1.text = precio1.ToString();
        precio4_2.text = precio2.ToString();
        precio4_3.text = precio3.ToString();
        // Asignamos el color a la imagen
        imagen4.color = color;
    }

    // Metodo para desactivar todas las opciones
    public void DesactivarOpciones()
    {
        // Desactivamos las cosas de la opcion 1
        imagen1.gameObject.SetActive(false);
        descripcion1.gameObject.SetActive(false);
        precio1_1.gameObject.SetActive(false);
        recurso1_1.gameObject.SetActive(false);
        precio1_2.gameObject.SetActive(false);
        recurso1_2.gameObject.SetActive(false);
        precio1_3.gameObject.SetActive(false);
        recurso1_3.gameObject.SetActive(false);
        // Desactivamos las cosas de la opcion 2
        imagen2.gameObject.SetActive(false);
        descripcion2.gameObject.SetActive(false);
        precio2_1.gameObject.SetActive(false);
        recurso2_1.gameObject.SetActive(false);
        precio2_2.gameObject.SetActive(false);
        recurso2_2.gameObject.SetActive(false);
        precio2_3.gameObject.SetActive(false);
        recurso2_3.gameObject.SetActive(false);
        // Desactivamos las cosas de la opcion 3
        imagen3.gameObject.SetActive(false);
        descripcion3.gameObject.SetActive(false);
        precio3_1.gameObject.SetActive(false);
        recurso3_1.gameObject.SetActive(false);
        precio3_2.gameObject.SetActive(false);
        recurso3_2.gameObject.SetActive(false);
        precio3_3.gameObject.SetActive(false);
        recurso3_3.gameObject.SetActive(false);
        // Desactivamos las cosas de la opcion 4
        imagen4.gameObject.SetActive(false);
        descripcion4.gameObject.SetActive(false);
        precio4_1.gameObject.SetActive(false);
        recurso4_1.gameObject.SetActive(false);
        precio4_2.gameObject.SetActive(false);
        recurso4_2.gameObject.SetActive(false);
        precio4_3.gameObject.SetActive(false);
        recurso4_3.gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;

public class poderI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto colisiona con el jugador, se activa la inmunidad y cambia de color
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(cambioDeColorImnunidad(other.gameObject));
        }
    }

    // Poder de inmunidad y cambio de color
    IEnumerator cambioDeColorImnunidad(GameObject player)
    {
        // Desactivar el objeto
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
        // Obtener el script de chocarEnemigo
        chocarEnemigo ce = player.GetComponent<chocarEnemigo>();
        if (ce != null)
        {
            // Activar la inmunidad
            ce.setInmunidad(true);
        }
        // Obtener los renderers del jugador
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        Color[] coloresOriginales = new Color[renderers.Length];
        if (renderers.Length > 0)
        {
            // Guardar los colores originales
            for (int i = 0; i < renderers.Length; i++)
            {
                coloresOriginales[i] = renderers[i].material.color;
            }
            // Cambiar los colores a amarillo
            Color nuevoColor = Color.yellow;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = nuevoColor;
            }
        }
        // Esperar 4 segundos
        yield return new WaitForSeconds(4.0f);
        if (renderers.Length > 0)
        {
            // Restaurar los colores originales
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = coloresOriginales[i];
            }
        }
        if (ce != null)
        {
            // Desactivar la inmunidad
            ce.setInmunidad(false);
        }
        // Destruir este objeto
        Destroy(this.gameObject);
    }
}

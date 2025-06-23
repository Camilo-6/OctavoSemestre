using UnityEngine;
using TMPro;
using System.Collections;

public class MostrarTextoSegundos : MonoBehaviour
{
    // Texto a mostrar
    [SerializeField] private TMP_Text texto;
    // Corrutina para mostrar el texto
    private Coroutine mostrarTextoCoroutine;

    // Metodo para mostrar el texto
    public void MostrarTexto(float segundos)
    {
        // Si ya hay una corrutina en ejecucion, la detenemos
        if (mostrarTextoCoroutine != null)
        {
            StopCoroutine(mostrarTextoCoroutine);
        }

        // Iniciamos la corrutina para mostrar el texto
        mostrarTextoCoroutine = StartCoroutine(MostrarTextoCoroutine(segundos));
    }

    // Corrutina para mostrar el texto durante un tiempo especificado
    IEnumerator MostrarTextoCoroutine(float segundos)
    {
        // Activamos el texto
        texto.gameObject.SetActive(true);
        texto.color = Color.red;
        // Esperamos el tiempo especificado
        yield return new WaitForSeconds(segundos);
        // Desactivamos el texto
        texto.gameObject.SetActive(false);
    }
}

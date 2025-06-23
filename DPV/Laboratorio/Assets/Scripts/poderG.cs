using UnityEngine;
using System.Collections;

public class poderG : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto colisiona con el jugador, se activa el aumento de tamanio
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(tamanioGrande(other.gameObject));
        }
    }

    // Poder de tamanio grande
    IEnumerator tamanioGrande(GameObject player)
    {
        // Desactivar el objeto
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
        // Obtener el transform del jugador
        Transform playerTransform = player.GetComponent<Transform>();
        // Obtener el characterController del jugador
        CharacterController cc = player.GetComponent<CharacterController>();
        // Aumentar el tamanio del jugador a x1.2
        if (playerTransform != null)
        {
            playerTransform.localScale = playerTransform.localScale * 1.2f;
        }
        // Aumentar el radio del characterController
        if (cc != null)
        {
            cc.radius = cc.radius * 1.2f;
        }
        // Esperar 5 segundos?
        yield return new WaitForSeconds(5.0f);
        // Resetear el tamanio del jugador
        if (playerTransform != null)
        {
            playerTransform.localScale = playerTransform.localScale / 1.2f;
        }
        // Resetear el radio del characterController
        if (cc != null)
        {
            cc.radius = cc.radius / 1.2f;
        }
        // Destruir este objeto
        Destroy(this.gameObject);
    }
}
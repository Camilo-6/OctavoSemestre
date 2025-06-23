using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class poderV : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        // Si el objeto colisiona con un jugador, se activa el poder
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(superVelocidad(other.gameObject));
            // Destroy(this.gameObject, 3.0f);
        }
    }

    // Poder de super velocidad
    IEnumerator superVelocidad(GameObject player)
    {
        // Desactivar el objeto
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
        // Lo siguiente es para el cerdito
        // Obtener el script de movimientoPersonaje
        movimientoPersonaje mp = player.GetComponent<movimientoPersonaje>();
        // Aumentar la velocidad del personaje
        if (mp != null)
        {
            mp.setVelocidad(5.0f);
        }
        // Obtener el script de movimientoBueno
        movimientoBueno mb = player.GetComponent<movimientoBueno>();
        // Almacenar la velocidad actual del personaje
        if (mb != null)
        {
            // Aumentar la velocidad del personaje
            mb.setVelocidad(10.0f);
        }
        // Esperar 5 segundos
        yield return new WaitForSeconds(5.0f);
        // Resetear la velocidad del personaje
        if (mp != null)
        {
            mp.resetVelocidad();
        }
        if (mb != null)
        {
            mb.resetVelocidad();
        }
        // Destruir este objeto
        Destroy(this.gameObject);
    }
}

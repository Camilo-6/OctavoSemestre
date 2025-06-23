using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    // Vida
    public Vida vida;
    // Collider
    [SerializeField] private Collider2D col;

    // Update is called once per frame
    void Update()
    {
        // Al hacer click izquierdo y este sobre el objeto
        if (Input.GetMouseButtonDown(0) && col.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            vida.PerderVida(10);
            Debug.Log("Vida: " + vida.vidaActual);
        }
    }

}
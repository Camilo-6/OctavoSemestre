using UnityEngine;
using UnityEngine.SceneManagement;

public class chocarEnemigo : MonoBehaviour
{
    // Inmunidad
    [SerializeField] private bool inmunidad = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inmunidad = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Si el objeto colisiona con un enemigo
        if (collision.gameObject.tag == "enemigo")
        {
            if (!inmunidad)
            {
                /*
                // Desanidar la camara antes de destruir el jugador
                Camera.main.transform.SetParent(null);
                // Mover la camara a (0, 0.673, 0.045)
                Camera.main.transform.position = new Vector3(0, 0.673f, 0.045f);
                // Rotar la camara a (0, 0, 0)
                Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
                // Destruir el jugador
                Destroy(gameObject);
                */
                // Cambiar la escena a la escena de Game Over
                SceneManager.LoadScene("Fin");
            }
            else
            {
                // Si tiene inmunidad, se destruye el enemigo
                Destroy(collision.gameObject);
            }
        }
    }

    // Setter de la inmunidad
    public void setInmunidad(bool inmunidad)
    {
        this.inmunidad = inmunidad;
    }
}

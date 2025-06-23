using UnityEngine;
using UnityEngine.SceneManagement;

public class managerPausa : MonoBehaviour
{
    // Pausa
    [SerializeField] GameObject pausa;
    // Booleano de pausa
    [SerializeField] bool pausado = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseGame();
        }
    }

    // Pausar juego
    public void pauseGame()
    {
        if (!pausado)
        {
            pausa.SetActive(true);
            pausado = true;
            Time.timeScale = 0;
        }
        else
        {
            pausa.SetActive(false);
            pausado = false;
            Time.timeScale = 1;
        }
    }

    // Salir al menu
    public void salirJuego()
    {
        pausa.SetActive(false);
        pausado = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}

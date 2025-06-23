using UnityEngine;

public class TorreExplosiva : TorreBasica
{
    // Radio de explosion
    [SerializeField] private float radioExplosion;

    // Metodo para atacar al enemigo
    protected override void Atacar()
    {
        // Instanciar el disparo
        GameObject disparo = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
        // Asignar el objetivo y radio al disparo
        DisparoExplosivo disparoScript = disparo.GetComponent<DisparoExplosivo>();
        if (disparoScript != null)
        {
            disparoScript.SetRadio(radioExplosion);
            disparoScript.SetDanio(danio);
            disparoScript.SetObjetivo(objetivoEnemigo);
        }
        // Revisamos si el danio del ataque mato al objetivo
        if (objetivoEnemigo != null)
        {
            EnemigoBasico enemigoScript = objetivoEnemigo.GetComponent<EnemigoBasico>();
            if (enemigoScript != null)
            {
                bool enemigoMuerto = enemigoScript.MoriraLuegoDeAtaque(danio);
                // Si el enemigo muere, quitarlo de la lista de enemigos en rango
                if (enemigoMuerto)
                {
                    enemigosEnRango.Remove(objetivoEnemigo);
                    objetivoEnemigo = null;
                }
            }
        }
    }
}

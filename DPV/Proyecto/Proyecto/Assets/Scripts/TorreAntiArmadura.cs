using UnityEngine;

public class TorreAntiArmadura : TorreBasica
{
    // Cantidad de armadura que el disparo quita al enemigo
    [SerializeField] private int menosArmadura;

    // Actualizar el metodo para obtener el enemigo a atacar
    // Metodo para obtener al enemigo a atacar
    protected override void ObtenerObjetivo()
    {
        // Limpar la lista de enemigos destruidos
        enemigosEnRango = enemigosEnRango.FindAll(e => e != null);
        // Si hay enemigos en rango
        if (enemigosEnRango.Count > 0)
        {
            // Obtener el enemigo con mayor armadura y usando la distancia al objetivo a defender como segunda prioridad
            int mayorArmadura = -1;
            float distanciaMinima = Mathf.Infinity;
            foreach (GameObject enemigo in enemigosEnRango)
            {
                // Revisamos la armadura del enemigo
                EnemigoBasico enemigoScript = enemigo.GetComponent<EnemigoBasico>();
                if (enemigoScript == null) continue;
                int armadura = enemigoScript.ObtenerArmadura();
                // Si la armadura del enemigo es mayor que la mayor armadura encontrada
                if (armadura > mayorArmadura)
                {
                    // Actualizar la mayor armadura y el objetivo
                    mayorArmadura = armadura;
                    objetivoEnemigo = enemigo;
                    distanciaMinima = Vector2.Distance(enemigo.transform.position, objetivoDefender.transform.position);
                }
                // Si la armadura es igual a la mayor armadura encontrada
                else if (armadura == mayorArmadura)
                {
                    // Calcular la distancia entre el enemigo y el objetivo a defender
                    float distancia = Vector2.Distance(enemigo.transform.position, objetivoDefender.transform.position);
                    // Si la distancia es menor que la minima
                    if (distancia < distanciaMinima)
                    {
                        // Actualizar la distancia minima y el objetivo
                        distanciaMinima = distancia;
                        objetivoEnemigo = enemigo;
                    }
                }
            }
        }
        else
        {
            // Si no hay enemigos en rango, limpiar el objetivo
            objetivoEnemigo = null;
        }
    }

    // Actualizar el metodo para atacar al enemigo
    protected override void Atacar()
    {
        // Instanciar el disparo
        GameObject disparo = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
        // Asignar el objetivo y la cantidad de armadura a quitar
        DisparoAntiArmadura disparoScript = disparo.GetComponent<DisparoAntiArmadura>();
        if (disparoScript != null)
        {
            disparoScript.SetMenosArmadura(menosArmadura);
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

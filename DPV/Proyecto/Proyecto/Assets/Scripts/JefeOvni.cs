using UnityEngine;

public class JefeOvni : EnemigoBasico
{
    // Prefab del ovni para atacar
    public GameObject prefabOvni;
    // Tiempo entre ataques
    public float tiempoEntreAtaques;
    // Tiempo desde el ultimo ataque
    private float tiempoDesdeUltimoAtaque;
    // Torre objetivo al que atacar
    private GameObject objetivo = null;

    // Update is called once per frame
    void Update()
    {
        // Si estamos en el estado 0
        if (estado == 0)
        {
            // Movemos al jefe
            MoverseEnParaleloAlCamino();
            // Aumentamos el tiempo desde el ultimo ataque
            tiempoDesdeUltimoAtaque += Time.deltaTime;
            // Si ha pasado el tiempo de ataque
            if (tiempoDesdeUltimoAtaque >= tiempoEntreAtaques)
            {
                // Atacamos con el ovni
                AtacarConOvni();
                // Reiniciamos el tiempo desde el ultimo ataque
                tiempoDesdeUltimoAtaque = 0.0f;
            }
        }

    }

    // Metodo para atacar con el ovni
    private void AtacarConOvni()
    {
        // Obtenemos el objetivo a atacar
        objetivo = ObtenerObjetivoOvni();
        // Si el objetivo es nulo
        if (objetivo == null)
        {
            // No hacemos nada
            return;
        }
        // Instanciamos el ovni en la posicion del jefe
        GameObject ovni = Instantiate(prefabOvni, transform.position, Quaternion.identity);
        // Asignamos el objetivo al ovni
        AtaqueOvni ataqueOvni = ovni.GetComponent<AtaqueOvni>();
        if (ataqueOvni != null)
        {
            ataqueOvni.AsignarTorreObjetivo(objetivo);
        }
    }

    // Metodo para obtener el objetivo al que atacar
    private GameObject ObtenerObjetivoOvni()
    {
        // Buscamos todas las torres en la escena
        ControladorTorre[] torres = FindObjectsByType<ControladorTorre>(FindObjectsSortMode.None);
        // Si no hay torres, no hacemos nada
        if (torres.Length == 0)
        {
            return null;
        }
        // Obtenemos la torre mas cercana
        ControladorTorre torreMasCercana = null;
        float distanciaMinima = float.MaxValue;
        foreach (ControladorTorre torre in torres)
        {
            // Si la torre esta activa
            if (torre.GetEstado() != 0)
            {
                // Calculamos la distancia al jefe
                float distancia = Vector3.Distance(transform.position, torre.transform.position);
                // Si es la mas cercana, la guardamos
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    torreMasCercana = torre;
                }
            }
        }
        // Regresamos la torre mas cercana
        return torreMasCercana != null ? torreMasCercana.gameObject : null;
    }

}

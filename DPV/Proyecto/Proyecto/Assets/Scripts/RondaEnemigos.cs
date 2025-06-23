using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RondaEnemigos
{
    // Cantidad de enemigos de cada tipo que habrá en esta ronda:
    public int enemigoBasicoCount;
    public int enemigoRapidoCount;
    public int enemigoPesadoCount;
    public int enemigoBasicoL2Count;
    public int enemigoRapidoL2Count;
    public int enemigoPesadoL2Count;
    public int enemigoArmaduraCount;
    public int enemigoArmaduraEnMuerteCount;
    public int enemigoJefeOvniCount;
    public float delayAfterRound; // Tiempo a esperar entre esta ronda y la siguiente
}
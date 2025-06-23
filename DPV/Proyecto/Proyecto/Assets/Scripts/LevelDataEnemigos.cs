using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelDataEnemigos", menuName = "Scriptable Objects/LevelDataEnemigos")]
public class LevelDataEnemigos : ScriptableObject
{
    // Nombre del nivel
    public string nombreDelNivel;
    // Lista de rondas del nivel
    public List<RondaEnemigos> rondas;
    // Camino que siguen los enemigos de las rondas
    [SerializeField] public GameObject caminoPrefab;
}

using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "GameData/Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Info")]
    public string id;
    public bool inPlace;
    public GameObject prefab;
    // Agregar una variable que defina el material de su bloque base jiji
}

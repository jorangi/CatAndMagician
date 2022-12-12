using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public StrGameObjDictionary Enemies = new();
    public void EnemySpawn(string enemyName)
    {
        GameObject enemy = Instantiate(Enemies[enemyName]);
        enemy.transform.position = new(Random.Range(-2.55f, 2.55f), 5.5f, 0);
        enemy.name = enemyName;
    }
    public void KillEnemies(string enemyName)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in enemies)
        {
            if(enemy.name == enemyName)
            {
                enemy.HP = 0;
            }
        }
    }
    public void RemoveEnemies(string enemyName)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if(enemy.name == enemyName)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}

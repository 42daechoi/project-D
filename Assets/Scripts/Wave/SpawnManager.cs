using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;  // 스폰 위치
    public Transform[] waypoints; // 웨이포인트 배열

    public IEnumerator SpawnWave(Wave wave)
    {
        int enemiesSpawned = 0;

        while (enemiesSpawned < wave.enemyCount)
        {
            GameObject monster = Instantiate(wave.enemies[0], spawnPoint.position, Quaternion.Euler(0, 180, 0));
            Monster monsterSet = monster.GetComponent<Monster>();
            if (monsterSet != null)
            {
                monsterSet.waypoints = waypoints;
            }

            enemiesSpawned++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}
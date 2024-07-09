using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;  // 스폰 위치
    public Transform[] waypoints; // 웨이포인트 배열
    
    public IEnumerator SpawnWave(Wave wave, List<Monster> spawnedMonsters, int waveIndex)
    {
        int enemiesSpawned = 0;
        string waveName = "Wave" + (waveIndex + 1).ToString("D2"); // 웨이브 이름 설정 ("Wave01", "Wave02", ...)
        GameObject monsterContainer = new GameObject(waveName);
        monsterContainer.transform.position = spawnPoint.position;

        while (enemiesSpawned < wave.enemyCount)
        {
            GameObject monsterObj = Instantiate(wave.enemies[0], spawnPoint.position, Quaternion.Euler(0, 180, 0));
            monsterObj.transform.SetParent(monsterContainer.transform);
            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                monster.waypoints = waypoints;
                spawnedMonsters.Add(monster);
            }

            enemiesSpawned++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}
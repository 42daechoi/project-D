using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public string monsterResourcePath = "Enemy_Resources";
    public string bossResourcePath = "Boss_Resources";
    
    public int numberOfWaves = 10;         // 웨이브 수를 지정할 수 있는 변수
    public Wave[] waves;                  // 모든 웨이브의 배열
    public SpawnManager spawnManager;     // 스폰 매니저 참조
    private int currentWaveIndex = 0;     // 현재 웨이브 인덱스
    private GameObject[] enemyPrefabs;
    private GameObject[] bossPrefab;
    private int waveInfo = 0;
    private List<Monster> spawnedMonsters = new List<Monster>();

    void Start()
    {
        waves = new Wave[numberOfWaves];  // 웨이브 배열 길이 설정
        LoadEnemies();
        StartCoroutine(StartNextWave());
    }

    void LoadEnemies()
    {
        // Resources 폴더에서 모든 프리팹 로드
        enemyPrefabs = Resources.LoadAll<GameObject>(monsterResourcePath);
        bossPrefab = Resources.LoadAll<GameObject>(bossResourcePath);

        // 각 웨이브에 프리팹 할당
        int waveIndex = 0;
        for (int i = 0; i < waves.Length; i++)
        {
            if ((i + 1) % 10 == 0)
            {
                waves[waveIndex] = new Wave
                {
                    enemies = new GameObject[] { bossPrefab[waveIndex % bossPrefab.Length] },
                    isBossWave = true
                };
            }
            else
            {
                waves[waveIndex] = new Wave
                {
                    enemies = new GameObject[] { enemyPrefabs[i % enemyPrefabs.Length] },
                    isBossWave = false
                };
            }

            waveIndex++;
        }
    }

    IEnumerator StartNextWave()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            yield return StartCoroutine(spawnManager.SpawnWave(currentWave, spawnedMonsters, currentWaveIndex));
            currentWaveIndex++;
            waveInfo = currentWaveIndex;
            Debug.Log(waveInfo); // 이건 그냥 테스트용 즉 웨이브정보를 이런식으로 할까해서 주석처리해놈
            yield return new WaitForSeconds(5f); // 다음 웨이브 시작 전 대기 시간
        }
    }
}
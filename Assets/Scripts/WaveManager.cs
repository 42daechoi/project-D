using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs;  // 스폰할 몬스터 프리팹 배열
    public Transform spawnPoint;  // 스폰 포인트
    public Transform[] waypoints;  // 웨이포인트 배열
    public float spawnInterval = 0.5f;  // 스폰 간격
    public float timeLimit = 30f;
    private float currentTime;
    private int lastDisplayedTime;

    private int monsterIndex = 0;
    private int spawnCount = 0;
    private const int maxSpawnsPerMonster = 60;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeLimit;
        lastDisplayedTime = Mathf.CeilToInt(currentTime); // 초기화 시 현재 시간을 정수로 저장
        StartCoroutine(StartTimer());
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator StartTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            int ceilCurrentTime = Mathf.CeilToInt(currentTime);
            // 초 단위로 시간이 변할 때마다 콘솔에 출력
            if (ceilCurrentTime != lastDisplayedTime)
            {
                Debug.Log($"Time: {ceilCurrentTime}");
                lastDisplayedTime = ceilCurrentTime; // 마지막으로 출력한 시간을 업데이트
            }

            yield return null;
        }

        Timeout();
    }
    
    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            
            GameObject selectedPrefab = monsterPrefabs[monsterIndex];
            GameObject monster = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.Euler(0, 180, 0));

            // 몬스터 값 세팅
            Monster monsterSet = monster.GetComponent<Monster>();
            if (monsterSet != null)
            {
                monsterSet.waypoints = waypoints;  // 웨이포인트 할당
            }
            

            
            spawnCount++;
            Debug.Log(spawnCount);  // 테스트 용도 나중에 지워야함

            
            if (spawnCount >= maxSpawnsPerMonster)
            {
                monsterIndex = (monsterIndex + 1) % monsterPrefabs.Length;
                spawnCount = 0;
            }

            // 스폰 간격 관련 코루틴
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void Timeout()
    {
        Debug.Log("타임아웃");
    }
}
